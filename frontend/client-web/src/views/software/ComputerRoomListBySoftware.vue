<template>
   <div class="container-content">
      <div class="header-title">
         <component :is="headerTitle" />
      </div>
      <div class="table-operations flex justify-between pt-4">
         <div class="operations-left"></div>
         <div class="operations-right flex gap-2">
            <a-button type="text" @click="refreshGrid">
               <template #icon>
                  <ReloadOutlined />
               </template>
            </a-button>
            <a-input v-model:value="searchText" :placeholder="$t('ComputerRoom.SearchListHint')" allow-clear
               style="width: 200px" />
         </div>
      </div>
      <div class="content">
         <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
         }" :data-source="dataSource" :pagination="pagination" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key === 'name'">
                  <router-link :to="{ name: 'ComputerRoomView', params: { id: record.id } }">
                     {{ record.name }}
                  </router-link>
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-dropdown :trigger="['click']">
                        <template #overlay>
                           <a-menu @click="handleMenuClick">
                              <a-menu-item key="1">
                                 <CheckCircleOutlined />
                                 Tải/cập nhật vào lần sử dụng tiếp theo
                              </a-menu-item>
                              <a-menu-item key="2">
                                 <StopOutlined />
                                 Tắt tự động tải/ cập nhật
                              </a-menu-item>
                           </a-menu>
                        </template>
                        <a-button round>
                           <template #icon>
                              <MoreOutlined />
                           </template>
                        </a-button>
                     </a-dropdown>
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
import { computerRoomService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
// ========== start state ==========
const props = defineProps({
   masterId: {
      type: Number,
      default: 0,
      required: true
   },
   masterData: {
      type: Object,
      default: null,
   }
})
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const headerTitle = computed(() => {
   return h('div', [
      'Thông tin sử dụng phần mềm ',
      h('strong', props.masterData.name),
      ' tại các phòng máy'
   ]);
});
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
         title: "Name",
         dataIndex: "name",
         key: "name",
         width: "80px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'name' && sorted.order,
         fixed: "left",
      },
      {
         title: "Số máy đã tải phần mềm",
         dataIndex: "numberComputerDowloaded",
         key: "numberComputerDowloaded",
         width: "120px",
      },
      {
         title: "Số máy đã cài đặt phần mềm",
         dataIndex: "numberComputerDowloaded",
         key: "numberComputerDowloaded",
         width: "120px",
      },
      {
         title: "Action",
         dataIndex: "operation",
         key: "operation",
         width: "40px",
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
      let rs = await computerRoomService.getListFilterBySoftware(props.masterId, pagingParam);
      if (rs && rs.success && rs.data) {
         let temp = rs.data.list?.map((item) => {
            item.numberComputerDowloaded = `${item.currentDowloadSoftware || 0}/${item.currentCapacity || 0
               }`;
            item.dataIndex = `${item.currentInstalledSoftware || 0}/${item.currentCapacity || 0
               }`;
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
 * reffresh grid
 */
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

const handleMenuClick = e => {
   console.log('click', e);
};
// ========== end methods ==========
</script>
<style lang="scss" scoped>
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