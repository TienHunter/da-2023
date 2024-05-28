<template>
  <a-list item-layout="horizontal" :data-source="datas">
    <template #renderItem="{ item }">
      <a-list-item>
        <a-list-item-meta :description="item.message">
          <template #title>
            <span>{{ item.logTime }}</span>
          </template>
          <!-- <template #avatar>
            <a-avatar src="https://joeschmoe.io/api/v1/random" />
          </template> -->
        </a-list-item-meta>
      </a-list-item>
    </template>
  </a-list>
</template>
<script setup>
import { computerHistoryService } from "@/api";
import { FormatDateKey } from "@/constants";
import { message } from "ant-design-vue";
import _ from "lodash";
import * as signalR from "@microsoft/signalr";
import moment from "moment";
import { onBeforeMount, ref } from "vue";
import { useRoute } from "vue-router";
// ========== start property ==========
const route = useRoute();
const datas = ref([]);
const dataClones = ref([]);

// ========== end property ==========

// ========== start lifecycle ==========

onMounted(() => {
  // lắng nghe socket
  const conn = signalR.HubConnectionBuilder().withUrl("https://localhost:44328/monitor-session")
    .build();
  conn.start()
    .then((ms) => {
      console.log("SignalR connection established:", ms);
      connection.invoke("Connect", route.params.id)
    })
    .catch((error) => {
      console.error("Error establishing SignalR connection:", error);
    });
  conn.on("ReceviceMessageConnect", (message) => {
    console.log("ReceviceMessageConnect:", message);
  })
  conn.on("ReceviceMessage", (message) => {
    console.log("ReceviceMessage:", message);
  })


}),
  onBeforeMount(async () => {
    // lấy danh sách
    try {
      let rs = await computerHistoryService.getAllByMonitorSessionId(
        route.params.id
      );
      if (rs?.success && rs?.data) {
        data.value = rs.data;
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
