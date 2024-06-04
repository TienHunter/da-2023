<template>
  <div class="container">
    <div class="toolbar flex justify-between py-4">
      <div class="left flex gap-4">
        <a-select v-model:value="filters.computers" mode="multiple" style="min-width: 160px;max-width:360px"
          placeholder="Chọn máy tính" :field-names="{ label: 'name', value: 'id' }" :filter-option="filterOption"
          :options="listComputers" @change="handleChangeSelectComputer"></a-select>

        <a-select v-model:value="filters.level" :field-names="{ label: 'value', value: 'id' }" style="width:160px;"
          placeholder="Chọn cấp độ" :options="listLevels" @change="handleChangeSelectLevel"></a-select>
      </div>
      <div class="right">
        <a-input v-model:value="filters.searchValue" placeholder="Nhập từ khóa" allow-clear style="width: 200px" />
      </div>
    </div>
    <div class="content">
      <a-list item-layout="horizontal" :data-source="dataClones">
        <template #renderItem="{ item }">
          <a-list-item>
            <div class=" flex justify-between w-full">
              <div class="left flex gap-2">
                <span class="font-bold">{{ moment(item.logTime).format(FormatDateKey.Default) }}</span>
                <span>{{ item.message }}</span>
              </div>
              <div class="right">
                <WarningFilled v-if="item.level == 2" class=" text-orange-500" />
                <CheckCircleFilled v-if="item.level == 1" class=" text-green-500" />
              </div>
            </div>

          </a-list-item>
        </template>
      </a-list>
    </div>

  </div>
</template>
<script setup>
import { computerHistoryService, computerService } from "@/api";
import { ActionTypeSocket, ComputerLevelLog, FormatDateKey } from "@/constants";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { message } from "ant-design-vue";
import _, { filter } from "lodash";
import moment from "moment";
import { onBeforeMount, onMounted, reactive, ref, watch } from "vue";
import { useRoute } from "vue-router";
// ========== start property ==========
const props = defineProps({
  data: {
    type: Object,
    default: null
  },
  isHasSession: {
    type: Boolean,
    default: false
  }
})
const route = useRoute();
const datas = ref([]);
const dataClones = ref([]);
const filters = reactive({
  computers: [],
  level: null,
  searchValue: ""
});
const listComputers = ref([]);
const listLevels = ref([
  {
    id: null,
    value: "Không chọn"
  },
  {
    id: ComputerLevelLog.Allow,
    value: "Cho phép"
  },
  {
    id: ComputerLevelLog.Warning,
    value: "Cảnh báp"
  },
  {
    id: ComputerLevelLog.Serious,
    value: "nghiêm trọng"
  }
]);
// ========== end property ==========

// ========== start lifecycle ==========

onMounted(() => {
  onUseSocket();
}),
  onBeforeMount(async () => {
    // lấy danh sách
    try {
      await loadData();
      await loadComputers();
    } catch (error) {
      console.log(error);
      message.error($t("UnknownError"));
    }
  });

watch(() => filters.searchValue, () => {
  debounceSearch()
})
// ========== end lifecycle ==========

// ========== start methods ==========
const onUseSocket = () => {

  // còn phiêm thì mới mở socket
  if (props.isHasSession) {
    // lắng nghe socket
    const conn = new HubConnectionBuilder().withUrl(`${process.env.VUE_APP_API_BASE_URL}/ws`).configureLogging(LogLevel.Information).withAutomaticReconnect().build();
    conn.start()
      .then(() => {
        console.log("SignalR connection established:");
        // kết nối theo id session
        conn.invoke("Connect", props.data.id)
        // kết nối theo id computerRoom
        conn.invoke("Connect", props.data.computerRoomId)
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
          case ActionTypeSocket.ADD_HISTORY:
            addHistoryFromSocket(res.message)
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
const debounceSearch = _.debounce(() => {
  filterData();
}, 600);
const loadData = async () => {
  try {
    let rs = await computerHistoryService.getAllByMonitorSessionId(
      props?.data?.id
    );
    if (rs?.success && rs?.data) {
      datas.value = rs.data;
      dataClones.value = _.cloneDeep(rs.data);

    }
  } catch (error) {
    console.log(error);
    message.error($t("UnknownError"));
  }
}
const loadComputers = async () => {
  try {
    if (props?.data?.computerRoomId) {
      const rs = await computerService.getListByComputerRoomId(props.data.computerRoomId, {});
      if (rs?.data) {
        listComputers.value = rs.data;
      }
    }
  } catch (error) {
    console.log(error);
  }
}
const handleChangeSelectComputer = (val) => {
  filterData();
}
const filterOption = (input, option) => {
  return option.name.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const handleChangeSelectLevel = (val) => {
  filterData();
}
const filterData = (item) => {
  if (item && checkFilter(item)) {
    dataClones.value.unshift(item);
  }
  dataClones.value = _.cloneDeep(datas.value.filter(i => checkFilter(i)));
}
const checkFilter = (i) => {
  if (i) {
    return (!filters.computers.length || filters.computers.includes(i.id)) && (!filters.level || filters.level == i.level) && (!filters.searchValue || i.message.includes(filters.searchValue.trim()))
  }
  return false;
}
const addHistoryFromSocket = (item) => {
  if (item) {
    datas.value.unshift(item);
    filterData(item);
  }
}
// ========== end methods ==========
</script>
<style lang="scss" scoped>
.container {
  .content {
    height: calc(100vh - 240px);
    overflow: auto;
  }

}
</style>