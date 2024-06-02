<template>
   <div class="container-content">
      <div class="header-title">
         <component :is="headerTitle" />
      </div>
      <div class="table-operations flex justify-between pt-4">
         <div class="operations-left">
            <div v-if="hasSelected" class="flex items-center gap-2">
               <a-button @click="unSelect">{{ $t("UnSelect") }}</a-button>
               <a-checkbox v-model:checked="selectAllComputer">{{ $t("SelectAll") }}</a-checkbox>
               <a-dropdown :trigger="['click']">
                  <template #overlay>
                     <a-menu>
                        <a-menu-item
                           @click="updateCommandOptionDowloadFile(null, menuKey.MULTI, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, true)">
                           <CheckCircleOutlined />
                           Bật tự dộng tải/cập nhật file cài đặt
                        </a-menu-item>
                        <a-menu-item
                           @click="updateCommandOptionDowloadFile(null, menuKey.MULTI, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, false)">
                           <StopOutlined />
                           Tắt tự động tải/ cập nhật file cài
                        </a-menu-item>
                        <a-menu-item
                           @click="updateCommandOptionDowloadFile(null, menuKey.MULTI, CommandOptionKey.CHECK_INSTALL_SOFTWARE, true)">
                           <CheckCircleOutlined />
                           Bật tự động kiểm tra phần mềm đã được cài đặt
                        </a-menu-item>
                        <a-menu-item
                           @click="updateCommandOptionDowloadFile(null, menuKey.MULTI, CommandOptionKey.CHECK_INSTALL_SOFTWARE, false)">
                           <StopOutlined />
                           Tắt tự động kiểm tra phần mềm đã được cài đặt
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
         </div>
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
         <a-table ref="tableRef" :columns="columns" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
         }" :data-source="dataSource" :pagination="pagination" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key === 'name'">
                  <router-link :to="{ name: 'ComputerView', params: { id: record.id } }">
                     {{ record.name }}
                  </router-link>
               </template>
               <template v-else-if="column.key === 'computerRoomName'">

                  {{ record?.computerRoom?.name }}
               </template>
               <template v-else-if="column.key === 'isDowloadFile'">
                  <template v-if="record?.computerSoftwares?.[0]?.isDowloadFile">
                     <CheckCircleOutlined style="color:green" />
                  </template>
                  <template v-else>
                     <StopOutlined />
                  </template>
               </template>
               <template v-else-if="column.key === 'isInstalled'">
                  <template v-if="record?.computerSoftwares?.[0]?.isInstalled">
                     <CheckCircleOutlined style="color:green" />
                  </template>
                  <template v-else>
                     <StopOutlined />
                  </template>
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-dropdown :trigger="['click']">
                        <template #overlay>
                           <a-menu>
                              <a-menu-item
                                 @click="updateCommandOptionDowloadFile(record, menuKey.SINGLE, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, true)">
                                 <CheckCircleOutlined />
                                 Bật tự dộng tải/cập nhật file cài đặt
                              </a-menu-item>
                              <a-menu-item v-has-permission="`${UserRole.Admin}`"
                                 @click="updateCommandOptionDowloadFile(record, menuKey.SINGLE, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, false)">
                                 <StopOutlined />
                                 Tắt tự động tải/ cập nhật file cài
                              </a-menu-item>
                              <a-menu-item
                                 @click="updateCommandOptionDowloadFile(record, menuKey.SINGLE, CommandOptionKey.CHECK_INSTALL_SOFTWARE, true)">
                                 <CheckCircleOutlined />
                                 Bật tự động kiểm tra phần mềm đã được cài đặt
                              </a-menu-item>
                              <a-menu-item
                                 @click="updateCommandOptionDowloadFile(record, menuKey.SINGLE, CommandOptionKey.CHECK_INSTALL_SOFTWARE, false)">
                                 <StopOutlined />
                                 Tắt tự động kiểm tra phần mềm đã được cài đặt
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
import { computed, h, nextTick, onBeforeMount, reactive, ref, watch, watchEffect } from "vue";
import { useRouter } from "vue-router";
import { commandOptionService, computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { CommandOptionKey, CommonKey, UserRole } from "@/constants";
// ========== start state ==========
const props = defineProps({
   masterId: {
      type: String,
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
      h('strong', props.masterData?.name),
      ' tại các máy'
   ]);
});
const loading = reactive({
   loadingTable: false,
   loadingInputSearch: false,
});
const menuKey = {
   SINGLE: "SINGLE",
   MULTI: "MULTI"
}
const filteredInfo = ref();
const sortedInfo = ref();
const columns = computed(() => {
   const filtered = filteredInfo.value || {};
   const sorted = sortedInfo.value || {};
   return [
      {
         title: $t("Computer.Name"),
         dataIndex: "name",
         key: "name",
         width: "80px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'name' && sorted.order,
         fixed: "left",
      },
      {
         title: $t("Computer.ComputerRoomName"),
         dataIndex: "computerRoomName",
         key: "computerRoomName",
         width: "120px",
      },
      {
         title: $t("Computer.IsDowloaded"),
         dataIndex: "isDowloadFile",
         key: "isDowloadFile",
         width: "40px",
      },
      {
         title: $t("Computer.IsInstalled"),
         dataIndex: "isInstalled",
         key: "isInstalled",
         width: "40px",
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
const tableRef = ref(null);
const scrollConfig = ref({
   y: "calc(100vh - 320px)"
});

const selectRows = reactive({
   selectedRowKeys: [],
});
const hasSelected = computed(() => {
   return selectRows.selectedRowKeys.length > 0
});
const selectAllComputerRoom = ref(false);
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
      let rs = await computerService.getListFilterBySoftware(props.masterId, pagingParam);
      if (rs && rs.success && rs.data) {
         let temp = rs.data.list?.map((item) => {
            item.numberComputerDowloaded = `${item.currentDowloadSoftware || 0}/${item.currentCapacity || 0
               }`;
            item.numberComputerInstalled = `${item.currentInstalledSoftware || 0}/${item.currentCapacity || 0
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
   selectRows.selectedRowKeys = []
   await loadData();
}

/**
 * tạo commandOption và cập nhật
 * @param record 
 * @param key 
 * @param value 
 */
const updateCommandOptionDowloadFile = async (record, key, keyOption, value) => {
   const userInfor = localStore.getItem(LocalStorageKey.userInfor);
   if (!(userInfor && userInfor.roleID == UserRole.Admin)) {
      message.warning($t("NotPermission"));
      return;
   }
   let conmandOption = null;
   switch (key) {
      case menuKey.SINGLE:
         conmandOption = {
            sourceIds: [record.id],
            desId: props.masterId,
            commandKey: keyOption,
            keyMapping: CommonKey.COMPUTER,
            commandValue: value
         }
         break;
      case menuKey.MULTI:
         if (selectAllComputerRoom.value) {
            conmandOption = {
               sourceIds: [],
               desId: props.masterId,
               commandKey: keyOption,
               keyMapping: CommonKey.ALL,
               commandValue: value

            }
         } else {
            conmandOption = {
               sourceIds: selectRows.selectedRowKeys,
               desId: props.masterId,
               commandKey: keyOption,
               keyMapping: CommonKey.COMPUTER,
               commandValue: value

            }
         }
      default:
         break;
   }
   await updateCommandOption(conmandOption, key);
};

/**
 * cập nhật command option
 * @param commandOption 
 */
const updateCommandOption = async (commandOption, key) => {
   try {
      if (commandOption) {
         let rs = await commandOptionService.upsertCommand(commandOption)
         if (rs && rs.success) {
            message.success($t("SaveSuccess"));
            if (key == menuKey.MULTI) {
               selectRows.selectedRowKeys = [];
               selectAllComputerRoom.value = false;
            }
         }
      }
   } catch (error) {
      console.log(error);
      message.error($t("UnknownError"));
   }
}
const unSelect = () => {
   selectRows.selectedRowKeys = [];
   selectAllComputerRoom.value = false;
}
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

::v-deep {}
</style>