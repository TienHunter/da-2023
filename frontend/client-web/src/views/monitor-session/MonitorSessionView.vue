<template>
   <div class="container-content">
      <div class="content h-full">
         <a-tabs v-model:activeKey="activeKey" :destroyInactiveTabPane="true" class="h-full">
            <template #leftExtra>
               <router-link :to="{ name: 'MonitorSessionList' }">
                  <a-button shape="circle" size="small" class="mr-2">
                     <template #icon>
                        <ArrowLeftOutlined />
                     </template>
                  </a-button>
               </router-link>
            </template>
            <a-tab-pane key="MonitorSessionInfoView">
               <template #tab>
                  <span>
                     <apple-outlined />
                     Thông tin chi tiết
                  </span>
               </template>
               <MonitorSessionInfoView :data="monitorSessionData" />
            </a-tab-pane>
            <a-tab-pane key="ComputerRoomComputerList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Danh sách máy
                  </span>
               </template>
               <ComputerRoomComputerList :computerRoomId="monitorSessionData.computerRoomId"
                  :useSocket="isHasSession" />
            </a-tab-pane>
            <a-tab-pane key="ComputerRoomMonitorSessionList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Lịch sử truy cập
                  </span>
               </template>
               <HisttoryAccess :data="monitorSessionData" :isHasSession="isHasSession" />
            </a-tab-pane>

         </a-tabs>
      </div>
   </div>
</template>
<script setup>
import { onBeforeMount, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import MonitorSessionInfoView from "./MonitorSessionInfoView.vue";
import ComputerRoomComputerList from "../computer-room/ComputerRoomComputerList.vue";
import HisttoryAccess from "./HisttoryAccess.vue";
import moment from "moment";
// ========== start state ========== 
const router = useRouter();
const route = useRoute();
const activeKey = ref("MonitorSessionInfoView");
const loading = reactive({
   spinning: false
})
const monitorSessionData = ref({});
const isHasSession = ref(false);
// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(() => {
   monitorSessionData.value = route.meta.data;
   if (monitorSessionData.value && monitorSessionData.value.endDate) {
      const endTimeSession = moment(monitorSessionData.value.endDate);
      const nowTime = moment();
      if (!endTimeSession.isAfter(nowTime)) {
         isHasSession.value = true;
      }
   }
   console.log(monitorSessionData.value);
})
// ========== end lifecycle ==========

// ========== start methods ========== 
// Your code here
// ========== end methods ==========
</script>
<style lang="scss" scoped>
.container-content {
   overflow: hidden !important;
}
</style>