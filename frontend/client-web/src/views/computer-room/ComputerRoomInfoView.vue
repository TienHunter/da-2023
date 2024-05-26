<template>
  <div class="container">
    <div class="toolbars flex justify-between py-4 rounded">
      <div class="toolbars-left"></div>
      <div class="toolbars-right flex gap-2">
        <router-link :to="{ name: 'ComputerRoomEdit', params: { id: route.params.id } }">
          <a-button type="primary" ghost>{{ $t("Edit") }}</a-button>
        </router-link>
      </div>
    </div>
    <div class="content">
      <div class="master">
        <a-row :gutter="[16, 24]">
          <template v-for="field in fields" :key="field.dataIndex">
            <a-col class="gutter-row" :span="6">
              <label class="font-bold">{{ field.title }}</label>
            </a-col>

            <a-col v-if="field.key == 'capacity'" class="gutter-row" :span="18">
              <div class="gutter-box">{{ data[field.key] }}</div>
            </a-col>
            <a-col v-else class="gutter-row" :span="18">
              <div class="gutter-box">{{ data[field.key] }}</div>
            </a-col>
          </template>
        </a-row>
        {{ data }}
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
import { ResponseCode } from "@/constants";
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
    title: "Layout",
    dataIndex: "layout",
    key: "layout",
  },
  {
    title: "Capacity",
    dataIndex: "capacity",
    key: "capacity",
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
      computerRoom.data.colorState = util.genColorState(
        "state",
        computerRoom.data.state
      );
      computerRoom.data.textState = util.genTextState(
        "state",
        computerRoom.data.state
      );
      computerRoom.data.capacity = `${computerRoom.data.currentCapacity || 0
        }/${computerRoom.data.maxCapacity || 0}`;
      computerRoom.data.colorPending = computerRoom.data.pending
        ? "orange"
        : "green";
      computerRoom.data.textPending = computerRoom.data.pending
        ? "chật"
        : "trống";
      computerRoom.data.layout = `${computerRoom.data.row || 0} x ${computerRoom.data.col || 0
        }`;
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
</script>
<style lang=""></style>
