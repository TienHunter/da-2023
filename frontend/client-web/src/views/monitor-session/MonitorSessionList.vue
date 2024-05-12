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
            <router-link :to="{ name: 'ComputerRoomAdd' }">
               <a-button type="primary">{{ $t("Add") }}</a-button>
            </router-link>
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
                     <a-button round>
                        <template #icon>
                           <EditOutlined />
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
import { FormatDateKey, MonitorType } from "@/constants";
import dayjs from "dayjs";
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
         width: "100px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'computerRoomName' && sorted.order,
         fixed: "left",
      },
      {
         title: $t("MonitorSession.MonitorTypeLabel"),
         dataIndex: "monitorType",
         key: "monitorType",
         width: "100px",
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
         filteredValue: filtered.monitorType || null,
         onFilter: (value, record) => record.monitorType.includes(value),
      },
      {
         title: $t("MonitorSession.StartDate"),
         dataIndex: "startDate",
         key: "startDate",
         width: "160px",
      },
      {
         title: $t("MonitorSession.EndDate"),
         dataIndex: "endDate",
         key: "endDate",
         width: "160px",
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
         title: "Action",
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
const scrollConfig = ref({ x: 1200, y: 400 });
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
            item.colorState = util.genColorState("state", item.state);
            item.textState = util.genTextState("state", item.state);
            item.computerRoomName = item?.computerRoom?.name;
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
         `Bạn có chắc chắn muốn xóa phòng máy ${record.name}.`,
         h("br"),
         `Khi xóa phòng
           máy thì thông tin các máy trong phòng và các thông tin liên quan sẽ bị
           xóa đi.`,
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await computerRoomService.delete(record.id);
            if (rs?.success && rs?.data) {
               message.success($t("ComputerRoom.DeleteSuccess", [record.name]));
               if (dataSource.value.length > 1) {
                  let indexToDelete = dataSource.value.findIndex(
                     (item) => item.id === record.id
                  );
                  if (indexToDelete != -1) {
                     dataSource.value.splice(indexToDelete, 1);
                     pagingParam.total -= 1;
                  }
               } else {
                  pagingParam.pageNumber = 1;
                  await loadData();
               }
            }
         } catch (errors) {
            message.error($t("UnKnowError"));
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
   await loadData();
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