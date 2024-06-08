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

        <a-button type="primary" v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="() => navigateAdd()">{{
          $t("Add") }}</a-button>
      </div>
    </div>
    <div class="content">
      <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
        selectedRowKeys: selectRows.selectedRowKeys,
        onChange: onSelectChange,
      }" :data-source="dataSource" :pagination="pagination" :loading="loading.loadingTable" :scroll="scrollConfig"
        @change="handleTableChange">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'macAddress'">
            <router-link :to="{ name: 'ComputerView', params: { id: record.id } }">
              {{ record.macAddress }}
            </router-link>
          </template>

          <template v-else-if="column.key === 'listError'">
            <div v-for="(tag, index) in record.listError" class="p-1" :key="index">
              <a-tag :color="tag.color">
                {{ tag.label }}
              </a-tag>
            </div>
          </template>
          <template v-else-if="column.key === 'computerRoomName'">
            <router-link :to="{ name: 'ComputerRoomView', params: { id: record.computerRoom.id } }">
              {{ record.computerRoom.name }}
            </router-link>
            <!-- <span>
              {{ record?.computerRoom?.name }}
            </span> -->
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
import { computed, onBeforeMount, reactive, ref, h, watch } from "vue";
import { useRouter } from "vue-router";
import moment from "moment";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import { ComputerKey, FormatDateKey, UserRole } from "@/constants";
// ========== start state ==========
const router = useRouter();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
  loadingTable: false,
  loadingInputSearch: false,
});
const searchText = ref("");
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
      width: "60px",
      sorter: (a, b) => a > b,
      sortOrder: sorted.columnKey === 'name' && sorted.order,
      fixed: "left",
    },
    {
      title: $t("Computer.MacAddress"),
      dataIndex: "macAddress",
      key: "macAddress",
      width: "120px",
    },
    {
      title: $t("ComputerRoom.Name"),
      dataIndex: "computerRoomName",
      key: "computerRoomName",
      width: "100px",
      sorter: (a, b) => a > b,
      sortOrder: sorted.columnKey === 'computerRoomName' && sorted.order,
    },
    {
      title: $t("Computer.Condition"),
      dataIndex: "listError",
      key: "listError",
      width: "100px",
    },
    {
      title: "Action",
      key: "operation",
      fixed: "right",
      width: 100,
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
const pagination = computed(() => ({
  total: pagingParam.total,
  current: pagingParam.pageNumber,
  pageSize: pagingParam.pageSize,
  showSizeChanger: true,
  showTotal: (total) => `Total ${total} items`,
}));
const scrollConfig = ref({ x: 1200, y: "calc(100vh - 240px)" });
let dataSource = ref([]);
const selectRows = reactive({
  selectedRowKeys: [],
});
const hasSelected = computed(() => {
  return selectRows.selectedRowKeys.length > 0
});
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
  try {
    loading.loadingTable = true;
    let rs = await computerService.getList(pagingParam);
    if (rs.success && rs.data) {
      dataSource.value = _.cloneDeep(rs.data.list?.map((item) => {
        item.listError = item?.listErrorId?.length > 0 ? item.listErrorId.map(errorId => {
          const { label, color } = util.handleRenderComputerError(errorId);
          return {
            value: errorId,
            label: label,
            color: color
          }
        }) : [{ value: ComputerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
        return item;
      }));

      pagingParam.total = rs.data.total || 0;
    }
  } catch (error) {
    console.log(error);
  } finally {
    loading.loadingTable = false;
  }
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
    let rs = await computerService.getList(pagingParam);
    if (rs.success && rs.data) {
      dataSource.value = _.cloneDeep(rs.data.list?.map((item) => {
        item.listError = item?.listErrorId?.length > 0 ? item.listErrorId.map(errorId => {
          const { label, color } = util.handleRenderComputerError(errorId);
          return {
            value: errorId,
            label: label,
            color: color
          }
        }) : [{ value: ComputerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
        return item;
      }));

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

const navigateAdd = () => {
  router.push({ name: "ComputerAdd" });
}

/**
 * navigate sang form edit
 * @param item 
 */
const navigateEdit = (item) => {
  // check something
  router.push({ name: "ComputerEdit", params: { id: item.id } });
}

/**
 * xóa bản ghi
 */
const onDelete = (record) => {
  modal.confirm({
    title: "Cảnh báo",
    icon: h(ExclamationCircleOutlined),
    content: h("div", [
      `Bạn có chắc chắn muốn xóa máy ${record.name} ở phòng máy ${record?.computerRoom?.name}.`
    ]),
    okText: "Yes",
    okType: "danger",
    async onOk() {
      try {
        let rs = await computerService.delete(record.id);
        if (rs?.success && rs?.data) {
          message.success($t("Computer.DeleteSuccess", [record.name, record?.computerRoom?.name]));
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

const refreshGrid = async () => {
  pagingParam.keySearch = "";
  pagingParam.pageNumber = 1;
  pagingParam.pageSize = 20;
  pagingParam.fieldSort = "UpdatedAt";
  pagingParam.sortAsc; false;
  searchText.value = "";
  await loadData();
}
/**
 * router sang form add
 */
// const onAdd = () => {
//   router.push({ name: "ComputerRoomAdd" });
// };
// const deleteMultiRecords = () => {
//   modal.confirm({
//     title: "Cảnh báo",
//     icon: h(ExclamationCircleOutlined),
//     content: h("div", [
//       `Bạn có chắc chắn muốn xóa ${selectRows.selectedRowKeys.length} máy.`
//     ]),
//     okText: $t("Yes"),
//     okText: $t("Cancel"),
//     okType: "danger",
//     async onOk() {
//       try {
//         let rs = await computerService.deleteRange(selectRows.selectedRowKeys);
//         if (rs?.success && rs?.data) {
//           message.success($t("DeleteSuccess"));
//           pagingParam.pageNumber = 1;
//           selectRows.selectedRowKeys = [];
//           await loadData();
//         }
//       } catch (errors) {
//         message.error($t("UnknownError"));
//         console.log(errors);
//       }
//     },
//     onCancel() { },
//   });
// }
// ========== end methods ==========
</script>
<style scoped>
.container-content {
  padding: 0 16px 16px 16px;
}

.table-operations {
  margin-bottom: 16px;
}

.table-operations>button {
  margin-right: 8px;
}
</style>
