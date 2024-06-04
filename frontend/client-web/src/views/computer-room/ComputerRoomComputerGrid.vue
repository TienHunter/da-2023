<template>
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left">
      </div>
      <div class="operations-right"></div>
    </div>
    <div class="body mx-10">
      <div class="content p-4 rounded-lg border-solid border-2 border-black">
        <a-row :gutter="[24, 24]" class="">
          <template v-for="row in computerRoom.row" :key="row">
            <a-col v-for="(col, index) in computerRoom.col" :key="col" :span="24 / computerRoom.col">
              <div class="flex flex-col items-center justify-center">
                <template v-if="dataRender[row] && dataRender[row][col]">
                  <div>
                    <a-badge :count="dataRender[row][col]['listErrorId']?.length" :overflow-count="10" class="pointer"
                      @click="quickViewComputer(dataRender[row][col]['id'])">

                      <LaptopOutlined class="text-6xl"
                        :class="{ 'text-blue-500': dataRender[row][col]?.computerState?.['state'], 'text-gray-500': !dataRender[row][col]?.computerState?.['state'] }" />

                    </a-badge>
                  </div>
                  <div class="font-bold pt-2 text-2xl">
                    {{ dataRender[row][col]['name'] }}
                  </div>
                  <div>

                  </div>
                </template>
                <template v-else>
                  <div class="flex items-center justify-center">
                    <a-button type="dashed" shape="cycle" v-has-permission="`${UserRole.Admin}`"
                      v-passPermissionClick="() => openQuickAddComputerModal(col, row)" :disabled="!props.isEditAble">
                      <template #icon>
                        <PlusOutlined />
                      </template>
                    </a-button>
                  </div>
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

  </div>
  <ComputerQuickAddModal v-if="computerPropAdd.isShow" :data="computerPropAdd"
    @toggleShowQuickAddComputerModal="(e) => toggleShowQuickAddComputerModal(e)"
    @afterAddComputer="(e) => afterAddComputer(e)" />
  <ComputerQuickViewModal v-if="computerPropView.isShow" :id="computerPropView.id" @hideModal="hideComputerQuickView" />
  <contextHolder />
</template>
<script setup>
import { computed, h, onBeforeMount, onUnmounted, reactive, ref, onBeforeUnmount, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import { computerRoomService, computerService } from "../../api";
import util from "@/utils/util";
import _ from "lodash";
import { Modal, message } from "ant-design-vue";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import moment from "moment";
import { ActionTypeSocket, FormatDateKey, UserRole } from "@/constants";
import ComputerQuickAddModal from "../computer/ComputerQuickAddModal.vue";
import ComputerQuickViewModal from "../computer/ComputerQuickViewModal.vue";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
// ========== start state ==========
const props = defineProps({
  computerRoomId: {
    type: String,
    default: "",
    required: true,
  },
  isEditAble: {
    type: Boolean,
    default: false
  },
  useSocket: {
    type: Boolean,
    default: false
  }
});

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
    let rs = await computerRoomService.getById(props.computerRoomId);
    if (rs && rs.success && rs.data) {
      computerRoom.value = rs.data;
    }

    await loadData();
    handleDataRender();
  } catch (error) {
    console.log(error);
    message.error($t("UnknownError"))
  }

});

onMounted(() => {
  onUseSocket();

  // chạy mỗi 5 phút
  interval.value = setInterval(async () => {
    autoUpdateState();
  }, 60000);
})
onBeforeUnmount(() => {
  clearInterval(interval.value); // Xóa interval khi component bị hủy
})
// watch(
//   () => dataSource.value,
//   () => {
//     handleDataRender();
//   },
//   { deep: true }
// );
// ========== end life cycle ==========

// ========== start methods ==========
const onUseSocket = () => {

  // còn phiêm thì mới mở socket
  if (props.useSocket) {
    // lắng nghe socket
    const conn = new HubConnectionBuilder().withUrl(`${process.env.VUE_APP_API_BASE_URL}/ws`).configureLogging(LogLevel.Information).withAutomaticReconnect().build();
    conn.start()
      .then(() => {
        console.log("SignalR connection established:");
        // kết nối theo id computerRoom
        conn.invoke("Connect", props.computerRoomId)
      })
      .catch((error) => {
        console.error("Error establishing SignalR connection:", error);
      });
    conn.on("ReceviceMessageConnect", (message) => {
      console.log("ReceviceMessageConnect:", message);
    })
    conn.on("ReceviceMessage", (res) => {
      try {
        switch (res.actionType) {
          case ActionTypeSocket.UPDATE_STATE_COMPUTER:
            updateComputerState(res.message)
            break;
          default:
            break;
        }
      } catch (error) {
        console.log(error);
      }
    })

  }

}

/**
 * cập nhật state cho máy tính
 */
const updateComputerState = (item) => {
  debugger;
  console.log(item);
  if (item) {
    let computer = dataSource.value?.find(c => c.id == item.computerId)
    if (computer) {
      computer.computerState = item;
    }

  }
}

const autoUpdateState = () => {
  console.log("workers update state");
  dataSource.value.forEach(element => {
    if (element && element.computerState && element.computerState.lastUpdate) {
      try {
        let latestUpdate = moment(element.computerState.lastUpdate);
        let now = moment();

        const differenceInMinutes = now.diff(latestUpdate, 'minutes');
        if (differenceInMinutes > 5) {
          element.computerState.state = false;
        }

      } catch (error) {
        console.log(error);
      }
    }
  });
}

const loadData = async () => {
  try {
    let rs = await computerService.getListByComputerRoomId(
      props.computerRoomId,
      {}
    );
    if (rs.success && rs.data) {
      dataSource.value = _.cloneDeep(rs.data);
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
    dataRender.value[element.row][element.col] = element;
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
          await loadData();

        }
      } catch (errors) {
        message.error($t("UnknownError"));
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
    message.error($t("UnknownError"));
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
    overflow: auto;
  }
}
</style>
