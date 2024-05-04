<template>
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left">
      </div>
      <div class="operations-right"></div>
    </div>
    <div class="content">
      <a-row :gutter="[24, 24]">
        <template v-for="row in computerRoom.row" :key="row">
          <a-col v-for="col in computerRoom.col" :key="col" :span="24 / computerRoom.col">
            <div class="flex flex-col items-center justify-center">
              <template v-if="dataRender[row] && dataRender[row][col]">
                <div>
                  <a-badge :count="dataRender[row][col]['listErrorId']?.length" :overflow-count="10" class="pointer"
                    @click="quickViewComputer(dataRender[row][col]['id'])">
                    <a-avatar shape="square" size="large" class="bg-green-500">
                      <template #icon>
                        <LaptopOutlined
                          :class="{ 'bg-blue-500': dataRender[row][col]['state'] == 1, 'bg-gray-500': dataRender[row][col]['state'] == 2 }" />
                      </template>
                    </a-avatar>
                  </a-badge>
                </div>
                <div class="font-bold pt-2 text-2xl">
                  {{ dataRender[row][col]['name'] }}
                </div>
                <div>

                </div>
              </template>
              <template v-else>
                <a-button type="dashed" shape="cycle" @click="openQuickAddComputerModal(col, row)">
                  <template #icon>
                    <PlusOutlined />
                  </template>
                </a-button>
              </template>
            </div>

          </a-col>
        </template>
      </a-row>
    </div>
    <div class=" py-2">
      {{ showTotal }}
    </div>
  </div>
  <ComputerQuickAddModal v-if="computerPropAdd.isShow" :data="computerPropAdd"
    @toggleShowQuickAddComputerModal="(e) => toggleShowQuickAddComputerModal(e)"
    @afterAddComputer="(e) => afterAddComputer(e)" />
  <ComputerQuickViewModal v-if="computerPropView.isShow" :id="computerPropView.id" @hideModal="hideComputerQuickView" />
  <contextHolder />
</template>
<script setup>
import { computed, h, onBeforeMount, onUnmounted, reactive, ref, onBeforeUnmount } from "vue";
import { useRoute, useRouter } from "vue-router";
import { computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import moment from "moment";
import { FormatDateKey } from "@/constants";
import ComputerQuickAddModal from "../computer/ComputerQuickAddModal.vue";
import ComputerQuickViewModal from "../computer/ComputerQuickViewModal.vue";
// ========== start state ==========
const router = useRouter();
const route = useRoute();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
  loadingInputSearch: false,
});
const dataSource = ref([]);
const dataRender = ref([]);
const computerRoom = ref({});
const showTotal = computed(
  () => `Total ${dataSource.value?.length || 0}/${computerRoom.value?.maxCapacity || 0}`
);
const pagingParam = reactive({
  fieldSort: "UpdatedAt",
  sortAsc: false,
});
const interval = ref(null);

const computerPropAdd = reactive({
  isShow: false,
  row: 0,
  col: 0,
})

const computerPropView = reactive({
  isShow: false,
  id: null
})
// ========== end state ==========

// ========== start life cycle ==========
onBeforeMount(async () => {
  try {
    let rs = await computerRoomService.getById(route.params.id);
    if (rs && rs.success && rs.data) {
      computerRoom.value = rs.data;
    }

    await loadData();
    handleDataRender();

    // interval.value = setInterval(async () => {
    //   await loadData();
    //   handleDataRender();
    // }, 3000); // Thực hiện mỗi 3 giây
  } catch (error) {
    console.log(error);
    message.error($t("UnKnowError"))
  }

});

onBeforeUnmount(() => {
  clearInterval(interval.value); // Xóa interval khi component bị hủy
})
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

const handleDataRender = () => {
  for (let row = 1; row <= computerRoom.value.row; row++) {
    dataRender.value[row] = [];
    for (let col = 1; col <= computerRoom.value.col; col++) {
      dataRender.value[row][col] = null;
    }
  }
  dataSource.value.forEach(element => {
    dataRender.value[element.row][element.col] = _.cloneDeep(element);
  });
}

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

const onBadge = () => {
  console.log("click here");
}

/**
 * mở modal thêm máy tính
 */
const openQuickAddComputerModal = (col, row) => {
  computerPropAdd.isShow = true;
  computerPropAdd.col = col;
  computerPropAdd.row = row;
}

/**
 * ẩn hiện modal thêm máy tính
 * @param {*} isShow 
 */
const toggleShowQuickAddComputerModal = (isShow) => {
  computerPropAdd.isShow = isShow;
}

/**
 * sau khi thêm máy tính thành công
 * @param {*} e 
 */
const afterAddComputer = async (e) => {
  try {
    computerPropAdd.isShow = false;
    computerPropAdd.col = 0;
    computerPropAdd.row = 0;
    await loadData();
    handleDataRender();
  } catch (error) {
    message.error($t("UnKnowError"));
  }
}

/**
 * xem nhanh thông tin máy
 * @param {*} id 
 */
const quickViewComputer = (id) => {
  if (id) {
    computerPropView.isShow = true;
    computerPropView.id = id;
  }
}

/**
 * ẩn component computerQuickView
 * @param {*} e 
 */
const hideComputerQuickView = (e) => {
  computerPropView.isShow = false;
}
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

  .content {
    max-height: calc(100vh - 240px);
    overflow: scroll;
  }
}
</style>