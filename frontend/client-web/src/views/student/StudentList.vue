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
               v-passPermissionClick="() => onOpenModalEdit(FormMode.Add)">{{
                  $t("Add")
               }}</a-button>
         </div>
      </div>
      <div class="content">
         <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
         }" :data-source="dataSource" :pagination="pagination" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key == 'createdAt' || column.key == 'updatedAt'">
                  {{ record[column.key] ? moment(record[column.key]).format(FormatDateKey.Default) : '-' }}
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-button round v-has-permission="`${UserRole.Admin}`"
                        v-passPermissionClick="() => onOpenModalEdit(FormMode.Update, record)">
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
   <StudentEdit v-if="propsModalEdit.isShow" :id="propsModalEdit.id" :formMode="propsModalEdit.formMode"
      @toggleShowModal="toggleShowEditModal" @afterSave="afterSaveModal" />
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { studentService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { FormMode, FormatDateKey, UserRole } from "@/constants";
import moment from "moment";
import StudentEdit from "./StudentEdit.vue";
// ========== start state ==========
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
   loadingTable: false,
   loadingInputSearch: false,
});
const filteredInfo = ref();
const sortedInfo = ref();
const propsModalEdit = reactive({
   isShow: false,
   id: "",
   formMode: FormMode.Add,
});
const columns = computed(() => {
   const filtered = filteredInfo.value || {};
   const sorted = sortedInfo.value || {};
   return [
      {
         title: $t("Student.StudentCode"),
         dataIndex: "studentCode",
         key: "studentCode",
         width: "80px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'studentCode' && sorted.order,
         fixed: "left",
      },
      {
         title: $t("Student.StudentName"),
         dataIndex: "studentName",
         key: "studentName",
         width: "120px",
         sorter: (a, b) => a > b,
         sortOrder: sorted.columnKey === 'studentName' && sorted.order,
         ellipsis: true,
      },
      {
         title: $t("Student.CreatedAt"),
         dataIndex: "createdAt",
         key: "createdAt",
         width: "120px",
         sorter: (a, b) => moment(a).diff(moment(b)),
         sortOrder: sorted.columnKey === 'createdAt' && sorted.order,
      },
      {
         title: $t("Student.UpdatedAt"),
         dataIndex: "updatedAt",
         key: "updatedAt",
         width: "120px",
         sorter: (a, b) => moment(a).diff(moment(b)),
         sortOrder: sorted.columnKey === 'updatedAt' && sorted.order,
      },
      {
         title: $t("Action"),
         dataIndex: "operation",
         key: "operation",
         width: "100px",
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
      let rs = await studentService.getList(pagingParam);
      if (rs && rs.success && rs.data) {

         dataSource.value = _.cloneDeep(rs.data.list);
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

const onOpenModalEdit = (type, record = null) => {
   propsModalEdit.isShow = true;
   propsModalEdit.formMode = type;
   propsModalEdit.id = record?.id
}
/**
 * xóa bản ghi
 */
const onDelete = (record) => {
   modal.confirm({
      title: "Cảnh báo",
      icon: h(ExclamationCircleOutlined),
      content: h("div", [
         `Bạn có chắc chắn muốn xóa sinh viên `,
         h("b", record.studentName),
         ' có MSSV: ',
         h("b", record.studentCode),
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await studentService.delete(record.id);
            if (rs?.success && rs?.data) {
               message.success($t("DeleteSuccess"));
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
         `Bạn có chắc chắn muốn xóa ${selectRows.selectedRowKeys.length} sinh viên đã chọn.`,
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await studentService.deleteRange(selectRows.selectedRowKeys);
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
const toggleShowEditModal = (e) => {
   propsModalEdit.isShow = e;
}
const afterSaveModal = async (e) => {
   try {
      propsModalEdit.isShow = false;
      if (e && e.data) {
         let rs = await studentService.getById(e.data);
         if (rs && rs.data) {
            if (propsModalEdit.formMode == FormMode.Add) {
               dataSource.value.unshift(rs.data);
            } else {
               let index = dataSource.value.indexOf(i => i.id = rs.data.id);
               if (index != -1) {
                  dataSource.value.splice(index, 1, rs.data);
               } else {
                  dataSource.value.unshift(rs.data);
               }
            }
         }
      }
   } catch (error) {
      console.log(error);
   }


}
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
}
</style>