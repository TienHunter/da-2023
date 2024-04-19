<template>
  <div class="contailer">
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
              Danh sach máy
            </span>
          </template>
          <ComputerRoomComputerList />
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

const router = useRouter();
const activeKey = ref("ComputerRoomInfoView");
</script>
<style lang=""></style>
