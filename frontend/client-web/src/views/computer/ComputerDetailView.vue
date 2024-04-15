<template>
  <div class="container">
    <div class="toolbars flex justify-between">
      <div class="toolbar-left"></div>
      <div class="toolbar-right">
        <router-link
          :to="{ name: 'ComputerEdit', params: { id: route.params.id } }"
        >
          <a-button type="primary" ghost>{{ $t("Edit") }}</a-button>
        </router-link>
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
        <a-col
          v-else-if="field.key == 'condition'"
          class="gutter-row"
          :span="18"
        >
          <div class="gutter-box">
            <a-tag :color="computerInfo.colorComputerCondition">
              {{ computerInfo.textComputerCondition }}
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
  import { ResponseCode, FormatDateKey } from "@/constants";
  import { util } from "@/utils";
  const route = useRoute();
  const router = useRouter();
  const fields = reactive([
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "ComputerRoomName",
      dataIndex: "computerRoomName",
      key: "computerRoomName",
    },
    {
      title: "MacAddress",
      dataIndex: "macAddress",
      key: "macAddress",
    },
    {
      title: "State",
      dataIndex: "state",
      key: "state",
    },
    {
      title: "StateTime",
      dataIndex: "stateTime",
      key: "stateTime",
    },
    {
      title: "Condition",
      dataIndex: "condition",
      key: "condition",
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
        const { colorComputerCondition, textComputerCondition } =
          util.getViewComputerCondition(computer.data.condition);
        computer.data.colorComputerCondition = colorComputerCondition;
        computer.data.textComputerCondition = textComputerCondition;
        computer.data.stateTime = computer.data.stateTime
          ? moment(computer.data.stateTime).format(FormatDateKey.Default)
          : "";
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
          message.error($t("UnKnowError"));
          break;
      }
      router.push({ name: "ComputerList" });
    } finally {
      loading.isLoadingBeforeMount = false;
    }
  });
</script>
<style></style>
