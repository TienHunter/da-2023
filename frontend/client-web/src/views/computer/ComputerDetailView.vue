<template>
  <div class="container">
    <div class="toolbars flex justify-between">
      <div class="toolbar-left"></div>
      <div class="toolbar-right">
        <a-button type="primary" ghost v-has-permission="`${UserRole.Admin}`"
          v-passPermissionClick="() => navigateEdit()">{{ $t("Edit") }}</a-button>

      </div>
    </div>
    <a-row :gutter="[16, 24]">
      <template v-for="field in fields" :key="field.dataIndex">
        <a-col class="gutter-row" :span="6">
          <label class="font-bold">{{ field.title }}</label>
        </a-col>
        <a-col v-if="field.key == 'state'" class="gutter-row" :span="18">
          <div class="gutter-box">
            <a-tag :color="computerInfo.colorComputerState">
              {{ computerInfo.textComputerState }}
            </a-tag>
          </div>
        </a-col>
        <a-col v-else-if="field.key == 'listError'" class="gutter-row" :span="18">
          <div class="gutter-box">
            <a-tag v-for="(tag, index) in computerInfo.listError" :key="index" :color="tag.color">
              {{ tag.label }}
            </a-tag>
          </div>
        </a-col>
        <a-col v-else class="gutter-row" :span="18">
          <div class="gutter-box">{{ computerInfo[field.key] }}</div>
        </a-col>
      </template>
    </a-row>
  </div>
</template>
<script setup>
import { computerService, userService } from "@/api";
import { ref, reactive, onBeforeMount } from "vue";
import { useRoute, useRouter } from "vue-router";
import moment from "moment";
import _ from "lodash";
import { message } from "ant-design-vue";
import { ResponseCode, FormatDateKey, ComputerKey, UserRole } from "@/constants";
import { util } from "@/utils";
const route = useRoute();
const router = useRouter();
const fields = reactive([
  {
    title: $t("Computer.Name"),
    dataIndex: "name",
    key: "name",
  },
  {
    title: $t("Computer.ComputerRoomName"),
    dataIndex: "computerRoomName",
    key: "computerRoomName",
  },
  {
    title: $t("Computer.MacAddress"),
    dataIndex: "macAddress",
    key: "macAddress",
  },
  {
    title: $t("Computer.State"),
    dataIndex: "state",
    key: "state",
  },
  {
    title: $t("Computer.StateTime"),
    dataIndex: "stateTime",
    key: "stateTime",
  },
  {
    title: $t("Computer.Condition"),
    dataIndex: "listError",
    key: "listError",
  },
]);
let computerInfo = ref({});
const loading = reactive({
  isLoadingBeforeMount: false,
});
onBeforeMount(async () => {
  try {
    loading.isLoadingBeforeMount = true;
    let computer = await computerService.getById(route.params.id);
    if (computer?.success && computer?.data) {
      const { colorComputerState, textComputerState } =
        util.getViewComputerState(computer.data.state);
      computer.data.colorComputerState = colorComputerState;
      computer.data.textComputerState = textComputerState;

      computer.data.stateTime = computer.data.stateTime
        ? moment(computer.data.stateTime).format(FormatDateKey.Default)
        : "";
      computer.data.listError = computer.data?.listErrorId?.length > 0 ? computer.data.listErrorId.map(errorId => {
        const { label, color } = util.handleRenderComputerError(errorId);
        return {
          value: errorId,
          label: label,
          color: color
        }
      }) : [{ value: ComputerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
      computer.data.computerRoomName = computer.data.computerRoom?.name;
      computerInfo.value = _.cloneDeep(computer.data);
    }
  } catch (error) {
    console.log(error);
    switch (error.code) {
      case ResponseCode.NotFoundUser:
        message.error($t("User.NotFoundUser"));
        break;
      default:
        message.error($t("UnknownError"));
        break;
    }
    router.push({ name: "ComputerList" });
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const navigateEdit = () => {
  router.push({ name: "ComputerEdit", params: { id: route.params.id } })
}
</script>
<style></style>
