<template>
  <div class="container-content">
    <div class="content">
      <a-tabs v-model:activeKey="activeKey" :destroyInactiveTabPane="true">
        <template #leftExtra>
          <router-link :to="{ name: 'ComputerRoomList' }">
            <a-button shape="circle" size="small" class="mr-2">
              <template #icon>
                <ArrowLeftOutlined />
              </template>
            </a-button>
          </router-link>
        </template>
        <a-tab-pane key="ComputerRoomInfoView">
          <template #tab>
            <span>
              <apple-outlined />
              Thông tin chi tiết
            </span>
          </template>
          <ComputerRoomInfoView />
        </a-tab-pane>
        <a-tab-pane key="ComputerRoomComputerList">
          <template #tab>
            <span>
              <android-outlined />
              Danh sách máy
            </span>
          </template>
          <ComputerRoomComputerList />
        </a-tab-pane>
        <a-tab-pane key="ComputerRoomMonitorSessionList">
          <template #tab>
            <span>
              <android-outlined />
              Danh sách phiên theo dõi
            </span>
          </template>
          <MonitorSessionListByComputerRoom />
        </a-tab-pane>

      </a-tabs>
    </div>
  </div>
</template>
<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import ComputerRoomInfoView from "./ComputerRoomInfoView.vue";
import ComputerRoomComputerList from "./ComputerRoomComputerList.vue";
import MonitorSessionListByComputerRoom from "../monitor-session/MonitorSessionListByComputerRoom.vue";

const router = useRouter();
const activeKey = ref("ComputerRoomInfoView");
</script>
<style lang="scss" scoped>
.container-content {
  overflow: hidden !important;
}
</style>
