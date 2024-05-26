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
            <a-tab-pane key="SoftwareDetail">
               <template #tab>
                  <span>
                     <apple-outlined />
                     Thông tin chi tiết
                  </span>
               </template>
               <div class="software-detail">
                  <SoftwareDetail />
               </div>

            </a-tab-pane>
            <a-tab-pane key="ComputerRoomList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Thông tin phần mềm ở các phòng máy
                  </span>
               </template>
               <div class="computer-room-list-software">
                  <ComputerRoomListBySoftware :masterId="route.params.id" :masterData="route.meta.data" />
               </div>

            </a-tab-pane>
            <a-tab-pane key="ComputerRoomMonitorSessionList">
               <template #tab>
                  <span>
                     <android-outlined />
                     Thông tin phần mềm ở các máy
                  </span>
               </template>
               <ComputerListBySoftware />
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
.container-content {
   overflow: hidden !important;
   padding: 0;

   ::v-deep {
      .ant-tabs-nav {
         padding: 0px 16px;
      }

      .ant-tabs-content-holder {
         position: relative;
         // padding: 0 16px 16px 16px;
         // overflow-y: auto;
         height: 100%;
      }

      .info-detail {
         overflow-y: auto
      }

      .software-detail {
         height: calc(100vh - 120px);
      }
   }
}
</style>