<template>
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left">
      </div>
      <div class="operations-right"></div>
    </div>
    <div class="content">
      <a-table :columns="columns" :row-key="(record) => record.id" :row-selection="{
        selectedRowKeys: selectRows.selectedRowKeys,
        onChange: onSelectChange,
      }" :data-source="dataSource" :pagination="false" :scroll="scrollConfig" :loading="loading.loadingTable"
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
          <template v-else-if="column.key === 'listError'">
            <div v-for="(tag, index) in record.listError" class="p-1" :key="index">
              <a-tag :color="tag.color">
                {{ tag.label }}
              </a-tag>
            </div>
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
        <template #footer> {{ showTotal }} </template>
      </a-table>
    </div>
  </div>
  <contextHolder />
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { computerService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import moment from "moment";
import { ComputerKey, FormatDateKey } from "@/constants";
// ========== start state ==========

const props = defineProps({
  computerRoomId: {
    type: String,
    default: "",
    required: true
  },
  isEditAble: {
    type: Boolean,
    default: false
  }
});
const router = useRouter();
const route = useRoute();

const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
  loadingTable: false,
  loadingInputSearch: false,
});
const columns = computed(() => {

  return props.isEditAble ? [
    {
      title: $t("Computer.Name"),
      dataIndex: "name",
      key: "name",
      width: "100px",
    },
    {
      title: $t("Computer.MacAddress"),
      dataIndex: "macAddress",
      key: "macAddress",
      width: "150px",
      ellipsis: true,
    },
    {
      title: $t("Computer.ComputerRoomName"),
      dataIndex: "computerRoomName",
      key: "computerRoomName",
      width: "100px",
      ellipsis: true,
    },
    {
      title: $t("Computer.State"),
      dataIndex: "state",
      key: "state",
      width: "100px",
    },
    {
      title: $t("Computer.StateTime"),
      dataIndex: "stateTime",
      key: "stateTime",
      width: "150px",
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
  ] :
    [
      {
        title: $t("Computer.Name"),
        dataIndex: "name",
        key: "name",
        width: "100px",
      },
      {
        title: $t("Computer.MacAddress"),
        dataIndex: "macAddress",
        key: "macAddress",
        width: "150px",
        ellipsis: true,
      },
      {
        title: $t("Computer.ComputerRoomName"),
        dataIndex: "computerRoomName",
        key: "computerRoomName",
        width: "100px",
        ellipsis: true,
      },
      {
        title: $t("Computer.State"),
        dataIndex: "state",
        key: "state",
        width: "100px",
      },
      {
        title: $t("Computer.StateTime"),
        dataIndex: "stateTime",
        key: "stateTime",
        width: "150px",
      },
      {
        title: $t("Computer.Condition"),
        dataIndex: "listError",
        key: "listError",
        width: "100px",
      },

    ];
})
const dataSource = ref([]);
const showTotal = computed(
  () => `Total ${dataSource.value?.length || 0} items`
);
const scrollConfig = ref({ x: 1200, y: 360 });
const selectRows = reactive({
  selectedRowKeys: [],
});
const pagingParam = reactive({
  keySearch: "",
  fieldSort: "UpdatedAt",
  sortAsc: false,
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
    let rs = await computerService.getListByComputerRoomId(
      props.computerRoomId,
      pagingParam
    );
    if (rs.success && rs.data) {
      let datas = rs.data.map((item) => {
        const { colorComputerState, textComputerState } =
          util.getViewComputerState(item.state);
        item.colorComputerState = colorComputerState;
        item.textComputerState = textComputerState;
        item.stateTime = item.stateTime
          ? moment(item.stateTime).format(FormatDateKey.Default)
          : "";

        item.listError = item?.listErrorId?.length > 0 ? item.listErrorId.map(errorId => {
          const { label, color } = handleRenderComputerError(errorId);
          return {
            value: errorId,
            label: label,
            color: color
          }
        }) : [{ value: ComputerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
        return item;
      });
      dataSource.value = _.cloneDeep(datas);
    }
  } catch (error) {
    console.log(error);
  } finally {
    loading.loadingTable = false;
  }
};
const handleRenderComputerError = (errorId) => {
  let label = "", color = "";
  switch (errorId) {
    case ComputerKey.ComputerError.Perfect:
      label = $t("Computer.ComputerError.Perfect");
      color = "green";
      break;
    case ComputerKey.ComputerError.Hardware:
      label = $t("Computer.ComputerError.Hardware");
      color = "error";
      break;
    case ComputerKey.ComputerError.Software:
      label = $t("Computer.ComputerError.Software");
      color = "orange";
      break;
    case ComputerKey.ComputerError.Network:
      label = $t("Computer.ComputerError.Network");
      color = "blue";
      break;
    case ComputerKey.ComputerError.OS:
      label = $t("Computer.ComputerError.OS");
      color = "warning";
      break;
    case ComputerKey.ComputerError.Unknow:
      label = $t("Computer.ComputerError.Unknow");
      color = "red";
      break;
    default:
      break;
  }
  return { label, color }
}
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
 * xóa bản ghi
 */
const onDelete = (record) => {
  modal.confirm({
    title: "Cảnh báo",
    icon: h(ExclamationCircleOutlined),
    content: h("div", [`Bạn có chắc chắn muốn xóa máy ${record.name}.`]),
    okText: "Yes",
    okType: "danger",
    async onOk() {
      try {
        let rs = await computerService.delete(record.id);
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
// ========== end methods ==========
</script>
<style lang="scss" scoped>
.container {
  .table-operations {
    margin-bottom: 16px;
  }

  .table-operations>button {
    margin-right: 8px;
  }
}
</style>
