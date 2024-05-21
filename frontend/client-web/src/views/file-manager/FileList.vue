<template>
  <div class="container-content">
    <div class="table-operations flex justify-between pt-4">
      <div class="operations-left"></div>
      <div class="operations-right flex gap-2">
        <a-input-search v-model:value="pagingParam.keySearch" placeholder="input search text" style="width: 200px"
          :loading="loading.loadingInputSearch" @search="onSearch" />
        <router-link :to="{ name: 'FileAdd' }">
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
          <template v-else-if="column.key === 'softwareName'">
            {{ record?.software?.name }}
          </template>
          <template v-else-if="column.key === 'fileSize'">
            {{ `${record?.size / 1024}` }}
            <b>KB</b>
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
import { computerRoomService, fileService } from "@/api";
import util from "@/utils/util";
import _ from "lodash";
import moment from "moment";
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
    title: $t("File.FileName"),
    dataIndex: "fileName",
    key: "fileName",
    width: "300px",
    sorter: true,
    fixed: "left",
    ellipsis: true,
  },
  {
    title: $t("File.SoftwareName"),
    dataIndex: "softwareName",
    key: "softwareName",
    width: "160px",
    sorter: true,
    ellipsis: true,
  },
  {
    title: $t("File.Version"),
    dataIndex: "version",
    key: "version",
    width: "100px",
  },
  {
    title: $t("File.FileSize"),
    dataIndex: "fileSize",
    key: "fileSize",
    width: "160px",
  },
  {
    title: $t("File.CreatedAt"),
    dataIndex: "createdAt",
    key: "createdAt",
    width: "150px",
  },
  {
    title: $t("File.UpdatedAt"),
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
    let rs = await fileService.getList(pagingParam);
    dataSource.value = _.cloneDeep(rs.data.list);
    pagingParam.total = rs.data.total || 0;
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
      `Bạn có chắc chắn muốn xóa file cài ${record.fileName} của phần mềm ${record?.software.name}.`
    ]),
    okText: "Yes",
    okType: "danger",
    async onOk() {
      try {
        let rs = await fileService.delete(record.id);
        if (rs?.success && rs?.data) {
          message.success($t("fileService.DeleteSuccess", [record.fileName]));
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