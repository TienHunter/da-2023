<template>
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left">
      </div>
      <div class="operations-right"></div>
    </div>
    <div class="content">
      <a-row :gutter="[24,24]">
        <template v-for="col in computerRoom.col"  :key="col">
          <a-col v-for="row in computerRoom.row" :key="row" :span="24 / computerRoom.row">
            <div>Row: {{ row }}, Col: {{ col }}</div>
          </a-col>
        </template>
      </a-row>
    </div>
  </div>
  <contextHolder />
</template>
<script setup>
import { computed, h, onBeforeMount, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import moment from "moment";
import { FormatDateKey } from "@/constants";
// ========== start state ==========
const router = useRouter();
const route = useRoute();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
  loadingInputSearch: false,
});

const dataSource = ref([]);
const computerRoom = ref({});
const showTotal = computed(
  () => `Total ${dataSource.value?.length || 0} items`
);
const pagingParam = reactive({
  fieldSort: "UpdatedAt",
  sortAsc: false,
});
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
  try {
    let rs = await computerRoomService.getById(route.params.id);
    if (rs && rs.success && rs.data) {
      computerRoom.value = rs.data;
    }
    await loadData();
  } catch (error) {
    message.error($t("UnKnowError"))
  }

});
// ========== end life cycle ==========

// ========== start methods ==========
const loadData = async () => {
  try {
    let rs = await computerService.getListByComputerRoomId(
      route.params.id,
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
        return item;
      });
      dataSource.value = _.cloneDeep(datas);
    }
  } catch (error) {
    console.log(error);
  }
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
        message.error($t("UnKnowError"));
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
  height: 480px;
  overflow: scroll;

  .table-operations {
    margin-bottom: 16px;
  }

  .table-operations>button {
    margin-right: 8px;
  }
}
</style>
