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
              <ZoomInOutlined />
              Thông tin chi tiết
            </span>
          </template>
          <ComputerRoomInfoView />
        </a-tab-pane>
        <a-tab-pane key="ComputerRoomComputerList">
          <template #tab>
            <span>
              <desktop-outlined />
              Danh sách máy
            </span>
          </template>
          <ComputerRoomComputerList :computerRoomId="route.params.id" :use-socket="true" />
        </a-tab-pane>
        <a-tab-pane key="ComputerRoomMonitorSessionList">
          <template #tab>
            <span>
              <VideoCameraOutlined />
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
import { useRoute, useRouter } from "vue-router";
import ComputerRoomInfoView from "./ComputerRoomInfoView.vue";
import ComputerRoomComputerList from "./ComputerRoomComputerList.vue";
import MonitorSessionListByComputerRoom from "../monitor-session/MonitorSessionListByComputerRoom.vue";

const router = useRouter();
const route = useRoute();
const activeKey = ref("ComputerRoomInfoView");
</script>
<style lang="scss" scoped>
.container-content {
  overflow: hidden !important;
  padding: 0;

  :deep {
    .ant-tabs-nav {
      padding: 0px 16px;
    }

    .ant-tabs-content-holder {
      position: relative;
      padding: 0 16px 16px 16px;
      overflow-y: auto;
      height: 100%;
    }
  }

  .content {
    height: 100%;
  }
}
</style>
