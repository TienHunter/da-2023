<template>
  <div class="container-content">
    <div class="table-operations flex justify-between pt-4">
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
      <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="rowSelection"
        :data-source="dataSource" :pagination="pagination" :loading="loading.loadingTable" @change="handleTableChange">
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
              <a-button round :disabled="userContext?.id == record.id" @click="navigateEdit(record)">
                <template #icon>
                  <EditOutlined />
                </template>
              </a-button>
              <a-button round class="bg-red-200" :disabled="userContext?.id == record.id">
                <template #icon>
                  <DeleteOutlined />
                </template>
              </a-button>
              <a-dropdown :trigger="['click']" :disabled="userContext?.id == record.id">
                <a-button @click.prevent>
                  <MoreOutlined />
                </a-button>
                <template #overlay>
                  <a-menu>
                    <a-menu-item v-if="record.state != UserState.Active"
                      @click="changeStateUser(UserState.Active, record)">
                      {{ $t("User.UserState.Active") }}
                    </a-menu-item>
                    <a-menu-item v-if="record.state != UserState.Revoke"
                      @click="changeStateUser(UserState.Revoke, record)">
                      {{ $t("User.UserState.Revoke") }}
                    </a-menu-item>
                  </a-menu>
                </template>
              </a-dropdown>
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
import { LocalStorageKey, UserState } from "@/constants";
import { localStore } from "@/utils";
import { message } from "ant-design-vue";
// ========== start state ==========
const router = useRouter();
const userContext = ref({});
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
    width: 160,
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
    userContext.value = localStore.getItem(LocalStorageKey.userInfor);
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



const rowSelection = {
  onChange: (selectedRowKeys, selectedRows) => {
    console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
    selectRows.selectedRowKeys = selectedRowKeys;
  },
  getCheckboxProps: record => ({
    disabled: record.id == userContext.value.id,
  }),
};
/**
 * sự kiện tìm kiếm ở ô input search
 * @param {*} searchValue
 */
const onSearch = (searchValue) => {
  console.log("use value", searchValue);
};

/**
 * navigate sang form edit
 */
const navigateEdit = (item) => {
  // check before router
  router.push({ name: "UserEdit", params: { id: item.id } });
}

const changeStateUser = async (state, user) => {
  try {
    let rs = await userService.updateUserState(user.id, state);
    if (rs && rs.success && rs.data) {
      user.state = state;
      const { colorUserState, textUserState } = util.getViewUserState(
        user.state
      );
      user.colorUserState = colorUserState;
      user.textUserState = textUserState;
    }
  } catch (error) {
    console.log(error);
    message.error($t("UnKnowError"));
  }
}
// ========== end methods ==========
</script>
<style scoped>
.container-content {
  .table-operations {
    margin-bottom: 16px;
  }

  .table-operations>button {
    margin-right: 8px;
  }
}
</style>
