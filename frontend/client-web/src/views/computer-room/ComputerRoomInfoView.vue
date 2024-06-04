<template>
  <div class="container">
    <div class="toolbars flex justify-between py-4 rounded">
      <div class="toolbars-left"></div>
      <div class="toolbars-right flex gap-2">
        <a-button type="primary" ghost v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="navigateEdit">{{
          $t("Edit") }}</a-button>
      </div>
    </div>
    <div class="content">
      <div class="master">
        <a-row :gutter="[16, 24]">
          <template v-for="field in fields" :key="field.dataIndex">
            <a-col class="gutter-row" :span="6">
              <label class="font-bold">{{ field.title }}</label>
            </a-col>
            <a-col class="gutter-row" :span="18">
              <div class="gutter-box">{{ data[field.key] }}</div>
            </a-col>
          </template>
        </a-row>
      </div>
    </div>
  </div>
</template>
<script setup>
import { computerRoomService } from "@/api";
import { ref, reactive, onBeforeMount } from "vue";
import { useRoute, useRouter } from "vue-router";
import _ from "lodash";
import { message } from "ant-design-vue";
import { FormatDateKey, ResponseCode, UserRole } from "@/constants";
import { util } from "@/utils";
import moment from "moment";
const route = useRoute();
const router = useRouter();
const fields = reactive([
  {
    title: $t("ComputerRoom.Name"),
    dataIndex: "name",
    key: "name",
  },
  {
    title: $t("ComputerRoom.Row"),
    dataIndex: "row",
    key: "row",

  },
  {
    title: $t("ComputerRoom.Col"),
    dataIndex: "col",
    key: "col",
  },
  {
    title: $t("ComputerRoom.CurrentCapacity"),
    dataIndex: "capacity",
    key: "capacity",
  },
  {
    title: $t("ComputerRoom.CreatedAt"),
    dataIndex: "createdAt",
    key: "createdAt",
  },
  {
    title: $t("ComputerRoom.UpdatedAt"),
    dataIndex: "updatedAt",
    key: "updatedAt",
  },
]);
let data = ref({});
const loading = reactive({
  isLoadingBeforeMount: false,
});
onBeforeMount(async () => {
  try {
    loading.isLoadingBeforeMount = true;
    let computerRoom = await computerRoomService.getById(route.params.id);
    if (computerRoom?.success && computerRoom?.data) {
      computerRoom.data.capacity = `${computerRoom.data.currentCapacity || 0}/${computerRoom.data.maxCapacity || 0}`;
      computerRoom.data.createdAt = computerRoom.data.createdAt ? moment(computerRoom.data.createdAt).format(FormatDateKey.Default) : "";
      computerRoom.data.updatedAt = computerRoom.data.updatedAt ? moment(computerRoom.data.updatedAt).format(FormatDateKey.Default) : "";
      data.value = _.cloneDeep(computerRoom.data);
    }
  } catch (error) {
    console.log(error);
    switch (error.code) {
      case ResponseCode.NotFoundComputerRoom:
        message.error($t("ComputerRoom.Validate.NotFound"));
        break;
      default:
        message.error($t("UnknownError"));
        break;
    }
    router.push({ name: "ComputerRoomList" });
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const navigateEdit = () => {
  router.push({ name: 'ComputerRoomEdit', params: { id: route.params.id } });
}
</script>
<style lang=""></style>
