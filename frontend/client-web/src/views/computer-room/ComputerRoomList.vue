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
        :scroll="scrollConfig"
        :loading="loading.loadingTable"
        @change="handleTableChange"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'name'">
            <router-link
              :to="{ name: 'ComputerRoomView', params: { id: record.id } }"
            >
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
  import { computerRoomService } from "../../api";
  import util from "@/utils/util";
  import _ from "lodash";
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
      title: "Số dãy",
      dataIndex: "row",
      key: "row",
      width: "100px",
    },
    {
      title: "Số lượng máy trên 1 dãy",
      dataIndex: "col",
      key: "col",
      width: "200px",
    },
    {
      title: "Số máy",
      dataIndex: "capacity",
      key: "capacity",
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
      dataIndex: "operation",
      key: "operation",
      width: "100px",
      fixed: "right",
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
  const dataSource = ref([]);
  const scrollConfig = ref({ x: 800, y: 300 });
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
        let temp = rs.data.list?.map((item) => {
          item.colorState = util.genColorState("state", item.state);
          item.textState = util.genTextState("state", item.state);
          item.capacity = `${item.currentCapacity || 0}/${
            item.maxCapacity || 0
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
<style lang="scss" scoped>
  .container {
    overflow: hidden;
    height: 100%;
    .table-operations {
      margin-bottom: 16px;
    }

    .table-operations > button {
      margin-right: 8px;
    }
  }
</style>
