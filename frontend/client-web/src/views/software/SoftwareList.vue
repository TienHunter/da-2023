<template>
   <div class="container-content">
      <div class="table-operations flex justify-between pt-4">
         <div class="operations-left"></div>
         <div class="operations-right flex gap-2">
            <a-input-search v-model:value="pagingParam.keySearch" placeholder="input search text" style="width: 200px"
               :loading="loading.loadingInputSearch" @search="onSearch" />
            <router-link :to="{ name: 'SoftwareAdd' }">
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
               <template v-if="column.key === 'name'">
                  <router-link :to="{ name: 'SoftwareView', params: { id: record.id } }">
                     {{ record.name }}
                  </router-link>
               </template>
               <template v-else-if="column.key === 'isUpdate' || column.key === 'isInstall'">
                  <span v-if="record[column.key]">
                     <CheckCircleOutlined />
                  </span>
                  <span v-else>
                     <CloseCircleOutlined />
                  </span>
               </template>
               <template v-else-if="column.key === 'createdAt' || column.key === 'updatedAt'">
                  <span v-if="record[column.key]">
                     {{ moment(record[column.key]).format(FormatDateKey.Default) }}
                  </span>
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
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
import { computed, h, onBeforeMount, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import _ from "lodash";
import moment from "moment";
import { softwareService } from "../../api";
import util from "@/utils/util";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { FormatDateKey } from "@/constants";
// ========== start state ==========
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
   loadingTable: false,
   loadingInputSearch: false,
});
const columns = [
   {
      title: $t('Software.Name'),
      dataIndex: "name",
      key: "name",
      width: "200px",
      sorter: true,
      fixed: "left",
   },
   {
      title: $t('Software.IsUpdate'),
      dataIndex: "isUpdate",
      key: "isUpdate",
      width: "80px",
      align: "center"
   },
   {
      title: $t('Software.IsInstall'),
      dataIndex: "isInstall",
      key: "isInstall",
      width: "80px",
      align: "center"
   },
   {
      title: $t('Software.CreatedAt'),
      dataIndex: "createdAt",
      key: "createdAt",
      width: "150px",
   },
   {
      title: $t('Software.UpdatedAt'),
      dataIndex: "updatedAt",
      key: "updatedAt",
      width: "150px",
   },
   {
      title: "Action",
      dataIndex: "operation",
      key: "operation",
      width: "100px",
      fixed: "right",
   },
];
const pagingParam = reactive({
   pageSize: 10,
   pageNumber: 1,
   keySearch: "",
   fieldSort: "UpdatedAt",
   sortAsc: false,
   total: 0,
});
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
// ========== end life cycle ==========

// ========== start methods ==========
const loadData = async () => {
   try {
      loading.loadingTable = true;
      let rs = await softwareService.getList(pagingParam);
      if (rs.success && rs.data) {
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
   console.log(
      pag.pageSize,
      pag?.current,
      sorter.field,
      sorter.order,
      filters
   );

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
 * xóa bản ghi
 */
const onDelete = (record) => {
   modal.confirm({
      title: "Cảnh báo",
      icon: h(ExclamationCircleOutlined),
      content: h("div", [
         `Bạn có chắc chắn muốn xóa phần mềm ${record.name}.`,
         h("br"),
         `Các máy tính sẽ không được cài đặt hoặc cập nhật phần mềm này nữa.`,
      ]),
      okText: "Yes",
      okType: "danger",
      async onOk() {
         try {
            let rs = await softwareService.delete(record.id);
            if (rs?.success && rs?.data) {
               message.success($t("Software.DeleteSuccess", [record.name]));
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