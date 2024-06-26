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
        <a-input v-model:value="searchText" :placeholder="$t('ComputerRoom.SearchListHint')" allow-clear
          style="width: 200px" />
        <a-button type="primary" v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="navigateAdd">{{ $t("Add")
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
          <template v-if="column.key === 'name'">
            <router-link :to="{ name: 'ComputerRoomView', params: { id: record.id } }">
              {{ record.name }}
            </router-link>
          </template>
          <template v-else-if="column.key === 'state'">
            <span>
              <a-tag :color="record.colorState">
                {{ record.textState }}
              </a-tag>
            </span>
          </template>
          <template v-else-if="column.key === 'operation'">
            <div class="flex gap-2">
              <a-button round v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="() => navigateEdit(record)">
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
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { computerRoomService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { UserRole } from "@/constants";
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
      title: $t("ComputerRoom.Name"),
      dataIndex: "name",
      key: "name",
      width: "100px",
      sorter: (a, b) => a > b,
      sortOrder: sorted.columnKey === 'name' && sorted.order,
      fixed: "left",
    },
    {
      title: $t("ComputerRoom.Row"),
      dataIndex: "row",
      key: "row",
      width: "100px",

    },
    {
      title: $t("ComputerRoom.Col"),
      dataIndex: "col",
      key: "col",
      width: "160px",
      ellipsis: true,
    },
    {
      title: $t("ComputerRoom.CurrentCapacity"),
      dataIndex: "capacity",
      key: "capacity",
      width: "150px",
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
    let rs = await computerRoomService.getList(pagingParam);
    if (rs && rs.success && rs.data) {
      let temp = rs.data.list?.map((item) => {
        item.capacity = `${item.currentCapacity || 0}/${item.maxCapacity || 0
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

const navigateEdit = (record) => {
  router.push({ name: "ComputerRoomEdit", params: { id: record.id } });
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
      `Bạn có chắc chắn muốn xóa ${selectRows.selectedRowKeys.length} phòng máy đã chọn.`,
      h("br"),
      `Khi xóa phòng
          máy thì thông tin các máy trong phòng và các thông tin liên quan sẽ bị
          xóa đi.`,
    ]),
    okText: "Yes",
    okType: "danger",
    async onOk() {
      try {
        let rs = await computerRoomService.deleteRange(selectRows.selectedRowKeys);
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
const navigateAdd = () => {
  router.push({ name: "ComputerRoomAdd" })
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

  // .content {
  //   .ant-table-body {
  //     min-height: calc(100vh - 248px) !important;
  //     max-height: calc(100vh - 248px) !important;
  //     height: calc(100vh - 248px) !important;
  //   }
  // }
}
</style>
