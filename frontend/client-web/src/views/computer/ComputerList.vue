<template>
  <div class="container-content">
    <div class="table-operations flex justify-between">
      <div class="operations-left"></div>
      <div class="operations-right flex gap-2">
        <a-input-search v-model:value="pagingParam.keySearch" placeholder="input search text" style="width: 200px"
          :loading="loading.loadingInputSearch" @search="onSearch" />
        <router-link :to="{ name: 'ComputerRoomAdd' }">
          <a-button type="primary">{{ $t("Add") }}</a-button>
        </router-link>
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
          <template v-else-if="column.key === 'state'">
            <span>
              <a-tag :color="record.colorComputerState">
                {{ record.textComputerState }}
              </a-tag>
            </span>
          </template>
          <template v-else-if="column.key === 'condition'">
            <span>
              <a-tag :color="record.colorComputerCondition">
                {{ record.textComputerCondition }}
              </a-tag>
            </span>
          </template>
          <template v-else-if="column.key === 'computerRoomName'">
            <span>
              {{ record?.computerRoom?.name }}
            </span>
          </template>
          <template v-else-if="column.key === 'operation'">
            <div class="flex gap-2">
              <a-button round>
                <template #icon>
                  <EditOutlined />
                </template>
              </a-button>
              <a-button round class="bg-red-200">
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
</template>
<script setup>
import { computed, onBeforeMount, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import moment from "moment";
import { computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import { FormatDateKey } from "@/constants";
// ========== start state ==========
const router = useRouter();
const loading = reactive({
  loadingTable: false,
  loadingInputSearch: false,
});
const columns = [
  {
    title: "Name",
    dataIndex: "name",
    key: "name",
    width: "100px",
  },
  {
    title: "MacAddress",
    dataIndex: "macAddress",
    key: "macAddress",
    width: "150px",
  },
  {
    title: "ComputerRoomName",
    dataIndex: "computerRoomName",
    key: "computerRoomName",
    width: "100px",
  },
  {
    title: "State",
    dataIndex: "state",
    key: "state",
    width: "100px",
  },
  {
    title: "StateTime",
    dataIndex: "stateTime",
    key: "stateTime",
    width: "150px",
  },
  {
    title: "Condition",
    dataIndex: "condition",
    key: "condition",
    width: "100px",
  },
  {
    title: "Action",
    key: "operation",
    fixed: "right",
    width: 100,
  },
];
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
const scrollConfig = ref({ x: 1200, y: 400 });
let dataSource = ref([]);
const selectRows = reactive({
  selectedRowKeys: [],
});
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
  try {
    loading.loadingTable = true;
    let rs = await computerService.getList(pagingParam);
    if (rs.success && rs.data) {
      dataSource.value = rs.data.list?.map((item) => {
        const { colorComputerState, textComputerState } =
          util.getViewComputerState(item.state);
        item.colorComputerState = colorComputerState;
        item.textComputerState = textComputerState;
        const { colorComputerCondition, textComputerCondition } =
          util.getViewComputerCondition(item.condition);
        item.colorComputerCondition = colorComputerCondition;
        item.textComputerCondition = textComputerCondition;
        item.stateTime = item.stateTime
          ? moment(item.stateTime).format(FormatDateKey.Default)
          : "";
        return item;
      });

      pagingParam.total = rs.data.total || 0;
    }
  } catch (error) {
    console.log(error);
  } finally {
    loading.loadingTable = false;
  }
});
// ========== end life cycle ==========

// ========== start methods ==========

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
 * router sang form add
 */
// const onAdd = () => {
//   router.push({ name: "ComputerRoomAdd" });
// };
// ========== end methods ==========
</script>
<style scoped>
.table-operations {
  margin-bottom: 16px;
}

.table-operations>button {
  margin-right: 8px;
}
</style>
