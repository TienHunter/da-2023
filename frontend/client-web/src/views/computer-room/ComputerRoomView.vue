<template lang="">
  <div class="contailer">
    <div class="toolbars flex justify-between py-4 rounded">
      <div class="toolbars-left">
        <router-link :to="{ name: 'ComputerRoomList' }">
          <a-button shape="circle">
            <template #icon>
              <ArrowLeftOutlined />
            </template>
          </a-button>
        </router-link>
      </div>
      <div class="toolbars-right flex gap-2">
        <a-button type="primary" ghost :loading="loading.isLoadingSave">{{
          $t("Edit")
        }}</a-button>
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
              <div class="gutter-box">{{ datas[field.key] }}</div>
            </a-col>
          </template>
        </a-row>
      </div>
      <div>{{ datas }}</div>
    </div>
  </div>
</template>
<script setup>
  import { computerRoomService } from "@/api";
  import { ref, reactive, onBeforeMount } from "vue";
  import { useRoute, useRouter } from "vue-router";
  import { message } from "ant-design-vue";
  import { ResponseCode } from "@/constants";
  const route = useRoute();
  const router = useRouter();
  const fields = reactive([
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Capacity",
      dataIndex: "capacity",
      key: "capacity",
    },
    {
      title: "Pending",
      dataIndex: "pending",
      key: "pending",
    },
  ]);
  let datas = ref({});
  const loading = reactive({
    isLoadingBeforeMount: false,
  });
  onBeforeMount(async () => {
    try {
      loading.isLoadingBeforeMount = true;
      let computerRoom = await computerRoomService.getById(route.params.id);
      if (computerRoom?.success && computerRoom?.data) {
        datas.value = { ...computerRoom?.data };
      }
    } catch (error) {
      console.log(error);
      switch (error.code) {
        case ResponseCode.NotFoundComputerRoom:
          message.error($t("ComputerRoom.Validate.NotFound"));
          break;
        default:
          message.error($t("UnKnowError"));
          break;
      }
      router.push({ name: "ComputerRoomList" });
    } finally {
      loading.isLoadingBeforeMount = false;
    }
  });
</script>
<style lang=""></style>
