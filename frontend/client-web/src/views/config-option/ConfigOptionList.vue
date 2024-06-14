<template>
   <div class="container-content">
      <div class="table-operations flex justify-between pt-4">
         <div class="operations-left">
            <div v-if="hasSelected" class="flex items-center gap-4">
               <a-button @click="unSelect">{{ $t("UnSelect") }}</a-button>
               <span>
                  <template v-if="hasSelected">
                     {{ $t("SelectCount", [selectRows.selectedRowKeys.length]) }}
                  </template>
               </span>
               <a-button danger @click="deleteMultiRecords">
                  {{ $t("Delete") }}
               </a-button>
            </div>
         </div>
         <div class="operations-right flex gap-2">
            <a-button type="text" @click="refreshGrid">
               <template #icon>
                  <ReloadOutlined />
               </template>
            </a-button>
            <a-input v-model:value="searchText" :placeholder="$t('SearchHinht')" allow-clear style="width: 200px" />
            <a-button type="primary" v-has-permission="`${UserRole.Admin}`"
               v-passPermissionClick="() => openModalEdit(FormMode.Add)">{{
                  $t("Add") }}</a-button>
         </div>
      </div>
      <div class="content">
         <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
         }" :data-source="dataSource" :pagination="pagination" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key === 'isSystem'">
                  <CloseCircleOutlined v-show="!record.isSystem" />
                  <CheckCircleOutlined v-show="record.isSystem" />
               </template>
               <template v-else-if="column.key === 'isAgent'">
                  <CloseCircleOutlined v-show="!record.isAgent" />
                  <CheckCircleOutlined v-show="record.isAgent" />
               </template>

               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-button round v-has-permission="`${UserRole.Admin}`"
                        v-passPermissionClick="() => openModalEdit(FormMode.Update, record)">
                        <template #icon>
                           <EditOutlined />
                        </template>
                     </a-button>
                     <a-button round class="bg-red-200" v-has-permission="`${UserRole.Admin}`"
                        v-passPermissionClick="() => onDelete(record)">
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
   <ConfigOptionEditModal v-if="configOptionEditModal.isShow" :form-mode="configOptionEditModal.formMode"
      @toggleShow="toggleShowConfigOptionEditModal" @afterSave="afterSaveConfigOptionEditModal"
      :id="configOptionEditModal.id" />
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { configOptionService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { FormMode, FormatDateKey, UserRole } from "@/constants";
import ConfigOptionEditModal from "./ConfigOptionEditModal.vue";
import moment from "moment";
// ========== start state ==========
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const configOptionEditModal = reactive({
   isShow: false,
   formMode: FormMode.Add,
   id: ""
})
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
         title: $t("ConfigOption.OptionName"),
         dataIndex: "optionName",
         key: "optionName",
         width: "200px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'name' && sorted.order,
         fixed: "left",
      },
      {
         title: $t("ConfigOption.OptionValue"),
         dataIndex: "optionValue",
         key: "optionValue",
         width: "160px",
         ellipsis: true,
      },
      {
         title: $t("ConfigOption.IsSystem"),
         dataIndex: "isSystem",
         key: "isSystem",
         width: "80px",
      },
      {
         title: $t("ConfigOption.IsAgent"),
         dataIndex: "isAgent",
         key: "isAgent",
         width: "80px",
      },
      {
         title: $t("ConfigOption.Desc"),
         dataIndex: "desc",
         key: "desc",
         width: "200px",
      },
      {
         title: $t("Action"),
         dataIndex: "operation",
         key: "operation",
         width: "200px",
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
const hasSelected = computed(() => {
   return selectRows.selectedRowKeys.length > 0
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
      let rs = await configOptionService.getList(pagingParam);
      if (rs && rs.success && rs.data) {
         let temp = rs.data.list?.map((item) => {
            item.createdAt = item.createdAt ? moment(item.createdAt).format(FormatDateKey.Default) : "";
            item.updatedAt = item.updatedAt ? moment(item.updatedAt).format(FormatDateKey.Default) : "";
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

const openModalEdit = (formMode, record) => {
   if (formMode == FormMode.Add) {
      configOptionEditModal.isShow = true;
      configOptionEditModal.formMode = formMode;
   } else if (formMode == FormMode.Update && record) {
      configOptionEditModal.isShow = true;
      configOptionEditModal.formMode = formMode;
      configOptionEditModal.id = record.id;
   }
}
/**
 * xóa bản ghi
 */
const onDelete = (record) => {
   modal.confirm({
      title: "Cảnh báo",
      icon: h(ExclamationCircleOutlined),
      content: h("div", [
         `Bạn có chắc chắn muốn xóa thiết lập có key ${record.optionName}.`
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await configOptionService.delete(record.id);
            if (rs?.success && rs?.data) {
               message.success($t("DeleteSuccess", [record.name]));
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
            message.error($t("UnknownError"));
            console.log(errors);
         }
      },
      onCancel() { },
   });
};

const deleteMultiRecords = () => {
   modal.confirm({
      title: "Cảnh báo",
      icon: h(ExclamationCircleOutlined),
      content: h("div", [
         `Bạn có chắc chắn muốn xóa ${selectRows.selectedRowKeys.length} thiết lập đã chọn.`,
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await configOptionService.deleteRange(selectRows.selectedRowKeys);
            if (rs?.success && rs?.data) {
               message.success($t("DeleteSuccess"));
               pagingParam.pageNumber = 1;
               selectRows.selectedRowKeys = [];
               await loadData();
            }
         } catch (errors) {
            message.error($t("UnknownError"));
            console.log(errors);
         }
      },
      onCancel() { },
   });
}

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

const unSelect = () => {
   selectRows.selectedRowKeys = []
}

const toggleShowConfigOptionEditModal = (e) => {
   configOptionEditModal.isShow = e
}

const afterSaveConfigOptionEditModal = async (e) => {
   configOptionEditModal.isShow = false;
   if (!e || !e.formMode || !e.id) return;

   try {
      const rs = await configOptionService.getById(e.id);

      if (!rs || !rs.data) return;

      const { data } = rs;
      const formatDate = (date) => (date ? moment(date).format(FormatDateKey.Default) : "");

      data.createdAt = formatDate(data.createdAt);
      data.updatedAt = formatDate(data.updatedAt);

      if (e.formMode === FormMode.Add) {
         dataSource.value.unshift(data);
      } else if (e.formMode === FormMode.Update) {
         const index = dataSource.value.findIndex(item => item.id === e.id);
         if (index !== -1) {
            dataSource.value.splice(index, 1, data);
         } else {
            dataSource.value.unshift(data);
         }
      }
   } catch (error) {
      console.error(error);
      message.error($t("UnknownError"));
   }
};

// ========== end methods ==========
</script>
<style lang="scss" scoped>
.container-content {
   position: relative;
   padding: 16px;
   padding-top: 0px;

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