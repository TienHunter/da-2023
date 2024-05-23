<template>
   <div class="container-content">
      <div class="content h-full">
         <a-tabs v-model:activeKey="activeKey" :destroyInactiveTabPane="true" class="h-full">
            <template #leftExtra>
               <router-link :to="{ name: 'SoftwareList' }">
                  <a-button shape="circle" size="small" class="mr-2">
                     <template #icon>
                        <ArrowLeftOutlined />
                     </template>
                  </a-button>
               </router-link>
            </template>
            <a-tab-pane key="SoftwareDetail" class="info-detail">
               <template #tab>
                  <span>
                     <apple-outlined />
                     Thông tin chi tiết
                  </span>
               </template>
               <SoftwareDetail />
            </a-tab-pane>
            <a-tab-pane key="ComputerRoomList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Thông tin phần mềm ở các phòng máy
                  </span>
               </template>
               <ComputerRoomListBySoftware :masterId="route.params.id" :masterData="route.meta.data" />
            </a-tab-pane>
            <a-tab-pane key="ComputerRoomMonitorSessionList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Thông tin phần mềm ở các máy
                  </span>
               </template>
               <ComputeristBySoftware />
            </a-tab-pane>

         </a-tabs>
      </div>
   </div>
</template>
<script setup>
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import SoftwareDetail from "./SoftwareDetail.vue";
import ComputerListBySoftware from "./ComputerListBySoftware.vue";
import ComputerRoomListBySoftware from "./ComputerRoomListBySoftware.vue";

const router = useRouter();
const route = useRoute();
const activeKey = ref("SoftwareDetail");
</script>
<style lang="scss" scoped>
::v-deep {
   .container-content {
      overflow: hidden !important;




   }

   .ant-tabs-content-holder {
      overflow-y: auto
   }

   .info-detail {
      overflow-y: auto
   }
}
</style>