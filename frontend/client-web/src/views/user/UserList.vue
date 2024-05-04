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
      }" :data-source="dataSource" :pagination="pagination" :loading="loading.loadingTable"
        @change="handleTableChange">
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'username'">
            <router-link :to="{ name: 'UserView', params: { id: record.id } }">
              {{ record.username }}
            </router-link>
          </template>
          <template v-else-if="column.key === 'state'">
            <span>
              <a-tag :color="record.colorUserState">
                {{ record.textUserState }}
              </a-tag>
            </span>
          </template>
          <template v-else-if="column.key === 'roleID'">
            <span>
              <a-tag :color="record.colorUserRole">
                {{ record.textUserRole }}
              </a-tag>
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
import _ from "lodash";
import { computerRoomService, userService } from "../../api";
import util from "@/utils/util";
// ========== start state ==========
const router = useRouter();
const loading = reactive({
  loadingTable: false,
  loadingInputSearch: false,
});
const columns = [
  {
    title: "Username",
    dataIndex: "username",
    key: "username",
    width: "150px",
  },
  {
    title: "Email",
    dataIndex: "email",
    key: "email",
    width: "150px",
  },
  {
    title: "Fullname",
    dataIndex: "fullname",
    key: "fullname",
    width: "150px",
  },
  {
    title: "Role",
    dataIndex: "roleID",
    key: "roleID",
    width: "150px",
  },
  {
    title: "State",
    dataIndex: "state",
    key: "state",
    width: "150px",
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
  showTotal: (total) => `Total ${total} items`,
}));
let dataSource = ref([]);
const selectRows = reactive({
  selectedRowKeys: [],
});
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
  try {
    loading.loadingTable = true;
    let rs = await userService.getList(pagingParam);
    if (rs.success && rs.data) {
      dataSource.value = _.cloneDeep(
        rs.data.list?.map((item) => {
          const { colorUserState, textUserState } = util.getViewUserState(
            item.state
          );
          item.colorUserState = colorUserState;
          item.textUserState = textUserState;
          const { colorUserRole, textUserRole } = util.getViewUserRole(
            item.roleID
          );
          item.colorUserRole = colorUserRole;
          item.textUserRole = textUserRole;
          return item;
        })
      );

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
const handleTableChange = (pag, filters, sorter) => {
  console.log(
    pag.pageSize,
    pag?.current,
    sorter.field,
    sorter.order,
    ...filters
  );
  //  run({
  //    results: pag.pageSize,
  //    page: pag?.current,
  //    sortField: sorter.field,
  //    sortOrder: sorter.order,
  //    ...filters,
  //  });
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
