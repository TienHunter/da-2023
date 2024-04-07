<template lang="">
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left"></div>
      <div class="operations-right flex gap-2">
        <a-input-search
          v-model:value="pagingParam.keySearch"
          placeholder="input search text"
          style="width: 200px"
          :loading="loading.loadingInputSearch"
          @search="onSearch"
        />
        <router-link :to="{ name: 'ComputerRoomAdd' }">
          <a-button type="primary">{{ $t("Add") }}</a-button>
        </router-link>
      </div>
    </div>
    <div class="content">
      <a-table
        :columns="columns"
        :row-key="(record) => record.id"
        :row-selection="{
          selectedRowKeys: selectRows.selectedRowKeys,
          onChange: onSelectChange,
        }"
        :data-source="dataSource"
        :pagination="pagination"
        :loading="loading.loadingTable"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'name'">
            <a>
              {{ record.name }}
            </a>
          </template>
          <template v-else-if="column.key === 'state'">
            <span>
              <a-tag :color="record.colorState">
                {{ record.textState }}
              </a-tag>
            </span>
          </template>
        </template>
      </a-table>
    </div>
  </div>
</template>
<script setup>
  import { computed, onBeforeMount, reactive, ref } from "vue";
  import { useRouter } from "vue-router";
  import { computerRoomService } from "../../api";
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
      width: "200px",
    },
    {
      title: "CurrentCapacity",
      dataIndex: "currentCapacity",
      key: "currentCapacity",
      width: "150px",
    },
    {
      title: "MaxCapacity",
      dataIndex: "maxCapacity",
      key: "maxCapacity",
      width: "150px",
    },
    {
      title: "State",
      dataIndex: "state",
      key: "state",
      width: "150px",
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      width: "150px",
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
  let dataSource = reactive([]);
  const selectRows = reactive({
    selectedRowKeys: [],
  });
  // ========== end state ==========

  // ========== start life cycle ==========
  onBeforeMount(async () => {
    try {
      loading.loadingTable = true;
      let rs = await computerRoomService.getList(pagingParam);
      if (rs.success && rs.data) {
        dataSource = reactive(
          rs.data.list?.map((item) => {
            item.colorState = genColorState(item.State);
            item.textState = genTextState(item.State);
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
   * gen ra màu cho trường state
   * @param {*} state
   */
  const genColorState = (state) => {
    let color = "volcano";
    switch (color) {
      case 0:
        color = "volcano";
        break;
      case 1:
        color = "green";
        break;
      case 2:
        color = "geekblue";
        break;
      default:
        break;
    }
    return color;
  };

  /**
   * gen text cho trường state
   * @param {*} state
   */
  const genTextState = (state) => {
    let text = "Hỏng";
    switch (text) {
      case 0:
        text = "Hỏng";
        break;
      case 1:
        text = "Tốt";
        break;
      case 2:
        text = "Bảo trì";
        break;
      default:
        break;
    }
    return text;
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

  .table-operations > button {
    margin-right: 8px;
  }
</style>
