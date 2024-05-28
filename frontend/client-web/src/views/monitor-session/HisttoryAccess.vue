<template>
  <div class="container">
    <a-list item-layout="horizontal" :data-source="datas">
     <template #renderItem="{ item }">
       <a-list-item>
        <div class="content flex justify-between w-full" >
          <div class="left flex gap-2">
            <span class="font-bold">{{ moment(item.logTime).format(FormatDateKey.Default) }}</span>
             <span>{{ item.message }}</span>
          </div>
          <div class="right">
              <WarningFilled v-if="item.level==2" class=" text-orange-500"/>
              <CheckCircleFilled v-if="item.level==1" class=" text-green-500"/>
            </div>
        </div>

       </a-list-item>
     </template>
   </a-list>
  </div>
 </template>
 <script setup>
 import { computerHistoryService } from "@/api";
 import { FormatDateKey } from "@/constants";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
 import { message } from "ant-design-vue";
 import _ from "lodash";
 import moment from "moment";
 import { onBeforeMount, onMounted, ref } from "vue";
 import { useRoute } from "vue-router";
 // ========== start property ==========
 const props = defineProps({
   data: {
      type: Object,
      default: null
   }
})
 const route = useRoute();
 const datas = ref([]);
 const dataClones = ref([]);
 
 // ========== end property ==========
 
 // ========== start lifecycle ==========
 
 onMounted(() => {
   // lắng nghe socket
   const conn = new HubConnectionBuilder().withUrl("https://localhost:44313/monitor-session").configureLogging(LogLevel.Information).build();
   conn.start()
     .then(() => {
       console.log("SignalR connection established:");
       conn.invoke("Connect", props.data.id)
     })
     .catch((error) => {
       console.error("Error establishing SignalR connection:", error);
     });
   conn.on("ReceviceMessageConnect", (message) => {
     console.log("ReceviceMessageConnect:", message);
   })
   conn.on("ReceviceMessage", (message) => {
     try {
      const rs = JSON.parse(message);
      if(rs.Message) {
        const ha = JSON.parse(rs.Message)
        console.log(ha);
        datas.value.unshift(ha);
        dataClones.value.unshift(ha);
      }
      console.log(rs);
     } catch (error) {
      
     }
   })
 
 
 }),
   onBeforeMount(async () => {
     // lấy danh sách
     try {
       let rs = await computerHistoryService.getAllByMonitorSessionId(
         props?.data?.id
       );
       if (rs?.success && rs?.data) {
          datas.value = rs.data;
         dataClones.value = _.cloneDeep(rs.data);
 
       }
     } catch (error) {
       console.log(error);
       message.error($t("UnknownError"));
     }
   });
 // ========== end lifecycle ==========
 
 // ========== start methods ==========
 // Your code here
 // ========== end methods ==========
 </script>
 <style lang="scss" scoped></style>
 