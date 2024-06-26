<template>
   <div class="container-content">
      <div class="table-operations flex justify-between pt-4">
         <div class="operations-left"></div>
         <div class="operations-right flex gap-2">
            <a-button type="text" @click="refreshGrid">
               <template #icon>
                  <ReloadOutlined />
               </template>
            </a-button>
            <a-input v-model:value="searchText" placeholder="Tìm kiếm" allow-clear style="width: 200px" />
            <a-button type="primary" @click="navigateAdd()">{{ $t("Add") }}</a-button>
         </div>
      </div>
      <div class="content">
         <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
         }" :data-source="dataSource" :pagination="pagination" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key === 'computerRoomName'">
                  <router-link :to="{ name: 'ComputerRoomView', params: { id: record.computerRoomId } }">
                     {{ record.computerRoomName }}
                  </router-link>
               </template>
               <template v-else-if="column.key === 'monitorType'">

                  <a-tag v-show="record.monitorType == MonitorType.Practive" color="green">
                     {{ $t("MonitorSession.MonitorType.Practive") }}
                  </a-tag>
                  <a-tag v-show="record.monitorType == MonitorType.Exam" color="blue">
                     {{ $t("MonitorSession.MonitorType.Exam") }}
                  </a-tag>

               </template>
               <template v-else-if="column.key === 'state'">
                  {{ record?.state?.text }}
               </template>
               <template v-else-if="column.key === 'domains'">
                  <div v-for="(domain, index) in record.domains" :key="index">
                     <a-tag>
                        {{ domain }}
                     </a-tag>
                  </div>
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-button round @click="viewDetail(record)">
                        <template #icon>
                           <EyeOutlined />
                        </template>
                     </a-button>
                     <a-button round class="bg-red-200" @click="onDelete(record)">
                        <template #icon>
                           <DeleteOutlined />
                        </template>
                     </a-button>
                  </div>
               </template>
            </template>
         </a-table>
      </div>
   </div>
   <contextHolder />
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { computerRoomService, monitorSessionService } from "@/api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { FormatDateKey, MonitorStateTime, MonitorType } from "@/constants";
import dayjs from "dayjs";
import moment from "moment";
// ========== start state ==========
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
   loadingTable: false,
   loadingInputSearch: false,
});
const filteredInfo = ref();
const sortedInfo = ref();
const columns = computed(() => {
   const filtered = filteredInfo.value || {};
   const sorted = sortedInfo.value || {};
   return [
      {
         title: $t("MonitorSession.ComputerRoomName"),
         dataIndex: "computerRoomName",
         key: "computerRoomName",
         width: "160px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'computerRoomName' && sorted.order,
         fixed: "left",
      },
      {
         title: $t("MonitorSession.MonitorTypeLabel"),
         dataIndex: "monitorType",
         key: "monitorType",
         width: "160px",
         filters: [
            {
               text: $t("MonitorSession.MonitorType.Practive"),
               value: MonitorType.Practive,
            },
            {
               text: $t("MonitorSession.MonitorType.Exam"),
               value: MonitorType.Exam,
            },
         ],
         filterMultiple: false,
         onFilter: (value, record) => record.monitorType == value,
      },
      {
         title: $t("MonitorSession.State"),
         dataIndex: "state",
         key: "state",
         width: "120px",
         filters: [
            {
               text: $t("MonitorSession.StateType.Running"),
               value: MonitorStateTime.Running,
            },
            {
               text: $t("MonitorSession.StateType.YetStarted"),
               value: MonitorStateTime.YetStarted,
            },
            {
               text: $t("MonitorSession.StateType.Finished"),
               value: MonitorStateTime.Finished,
            },
         ],
         filterMultiple: false,
         onFilter: (value, record) => record?.state?.value == value,
      },
      {
         title: $t("MonitorSession.StartDate"),
         dataIndex: "startDate",
         key: "startDate",
         width: "160px",
         sorter: true
      },
      {
         title: $t("MonitorSession.EndDate"),
         dataIndex: "endDate",
         key: "endDate",
         width: "170px",
         sorter: true
      }, {
         title: $t("MonitorSession.Domain"),
         dataIndex: "domains",
         key: "domains",
         ellipsis: true

      },
      {
         title: $t("MonitorSession.OwnerIdText"),
         dataIndex: "ownerIdText",
         key: "ownerIdText",
         width: "150px",
      },
      {
         title: $t("Action"),
         dataIndex: "operation",
         key: "operation",
         width: "140px",
         fixed: "right",
      },
   ];
})
const pagingParam = reactive({
   pageSize: 20,
   pageNumber: 1,
   keySearch: "",
   fieldSort: "UpdatedAt",
   sortAsc: false,
   total: 0,
});
const searchText = ref("");
const pagination = computed(() => ({
   total: pagingParam.total,
   current: pagingParam.pageNumber,
   pageSize: pagingParam.pageSize,
   showSizeChanger: true,
   showTotal: (total) => `Total ${total} items`,
}));
const dataSource = ref([]);
const scrollConfig = ref({ x: 1200, y: "calc(100vh - 240px)" });
const selectRows = reactive({
   selectedRowKeys: [],
});
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
   await loadData();
});

watch(searchText, () => {
   pagingParam.keySearch = searchText.value;
   debounceSearch();
});
// ========== end life cycle ==========

// ========== start methods ==========

const debounceSearch = _.debounce(async () => {
   await loadData();
}, 600);
const loadData = async () => {
   try {
      loading.loadingTable = true;
      let rs = await monitorSessionService.getList(pagingParam);
      if (rs.success && rs.data) {
         let temp = rs.data.list?.map((item) => {
            item.computerRoomName = item?.computerRoom?.name;
            item.state = bindMonitorState(item);
            item.startDate = item.startDate ? dayjs(item.startDate).format(FormatDateKey.Default) : "";
            item.endDate = item.endDate ? dayjs(item.endDate).format(FormatDateKey.Default) : "";
            item.ownerIdText = item?.user?.fullname;

            return item;
         });
         dataSource.value = _.cloneDeep(temp);
         pagingParam.total = rs.data.total || 0;
      }
   } catch (error) {
      console.log(error);
   } finally {
      loading.loadingTable = false;
   }
};

const bindMonitorState = (item) => {
   const state = {};
   const now = moment();
   if (now.isBefore(moment(item.startDate))) {
      state.value = MonitorStateTime.YetStarted;
      state.text = $t("MonitorSession.StateType.YetStarted")
   } else if (now.isSameOrAfter(moment(item.endDate))) {
      state.value = MonitorStateTime.Finished;
      state.text = $t("MonitorSession.StateType.Finished")
   } else {
      state.value = MonitorStateTime.Running;
      state.text = $t("MonitorSession.StateType.Running")
   }
   return state;
}
/**
 * paging
 * @param {*} pag
 * @param {*} filters
 * @param {*} sorter
 */
const handleTableChange = async (pag, filters, sorter) => {
   console.log(
      pag.pageSize,
      pag?.current,
      sorter.field,
      sorter.order,
      filters
   );
   filteredInfo.value = filters;
   sortedInfo.value = sorter;
   pagingParam.pageNumber = pag?.current || 1;
   pagingParam.pageSize = pag.pageSize;
   pagingParam.fieldSort = sorter.field;
   pagingParam.sortAsc = sorter.order == "ascend" ? true : false;
   pagingParam.filters = util.stringifyValue(_.cloneDeep(filters));

   await loadData();
};

/**
 * sự kiện selected rows
 * @param {*} selectedRowKeys
 */
const onSelectChange = (selectedRowKeys) => {
   console.log("selectedRowKeys changed: ", selectedRowKeys);
   selectRows.selectedRowKeys = selectedRowKeys;
};

/**
 * sự kiện tìm kiếm ở ô input search
 * @param {*} searchValue
 */
const onSearch = (searchValue) => {
   console.log("use value", searchValue);
};

/**
 * xem chi tiết
 * @param item 
 */
const viewDetail = (item) => {
   router.push({ name: "MonitorSessionView", params: { id: item.id } })
}
/**
 * xóa bản ghi
 */
const onDelete = (record) => {
   modal.confirm({
      title: "Cảnh báo",
      icon: h(ExclamationCircleOutlined),
      content: h("div", [
         `Bạn có chắc chắn muốn xóa phiên giám sát ở phòng máy ${record.computerRoomName} `,
         h("br"),
         `Thời gian: ${record.startDate} - ${record.endDate}`,
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await monitorSessionService.delete(record.id);
            if (rs?.success && rs?.data) {
               message.success($t("DeleteSuccess"));
               pagingParam.pageNumber = 1;
               await loadData();

            }
         } catch (errors) {
            message.error($t("UnknownError"));
            console.log(errors);
         }
      },
      onCancel() { },
   });
};

const refreshGrid = async () => {
   filteredInfo.value = null;
   sortedInfo.value = null;
   pagingParam.keySearch = "";
   pagingParam.pageNumber = 1;
   pagingParam.pageSize = 20;
   pagingParam.fieldSort = "UpdatedAt";
   pagingParam.sortAsc; false;
   pagingParam.filters = null;
   searchText.value = "";
   await loadData();
}

const navigateAdd = () => {
   router.push({ name: "MonitorSessionAdd" })
}
// ========== end methods ==========
</script>
<style lang="scss">
.container-content {
   .table-operations {
      margin-bottom: 16px;
   }

   .table-operations>button {
      margin-right: 8px;
   }

   // .content {
   //   .ant-table-body {
   //     min-height: calc(100vh - 248px) !important;
   //     max-height: calc(100vh - 248px) !important;
   //     height: calc(100vh - 248px) !important;
   //   }
   // }
}
</style>