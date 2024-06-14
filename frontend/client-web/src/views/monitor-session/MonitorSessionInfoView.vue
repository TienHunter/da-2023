<template>
   <div class="container h-full bg-white p-4 rounded">
      <div class="content">
         <div class="master">
            <a-row :gutter="[16, 24]">
               <template v-for="field in fields" :key="field.dataIndex">
                  <a-col class="gutter-row" :span="6">
                     <label class="font-bold">{{ field.title }}</label>
                  </a-col>

                  <a-col v-if="field.key == 'computerRoomName'" class="gutter-row" :span="18">
                     <div class="gutter-box">{{ data[field.key] }}</div>
                  </a-col>

                  <a-col v-else-if="field.key == 'monitorType'" class="gutter-row" :span="18">
                     <div class="gutter-box">
                        <a-tag v-show="data.monitorType == MonitorType.Practive" color="green">
                           {{ $t("MonitorSession.MonitorType.Practive") }}
                        </a-tag>
                        <a-tag v-show="data.monitorType == MonitorType.Exam" color="blue">
                           {{ $t("MonitorSession.MonitorType.Exam") }}
                        </a-tag>
                     </div>
                  </a-col>
                  <a-col v-else-if="field.key == 'domains'" class="gutter-row" :span="18">
                     <div class="gutter-box">
                        <a-tag v-for="(domain, index) in data.domains" :key="index">
                           {{ domain }}
                        </a-tag>
                     </div>
                  </a-col>
                  <a-col v-else class="gutter-row" :span="18">
                     <div class="gutter-box">{{ data[field.key] }}</div>
                  </a-col>
               </template>
            </a-row>
            <!-- {{ data }} -->
         </div>
      </div>
   </div>
</template>
<script setup>
import { computerRoomService } from "@/api";
import { ref, reactive, onBeforeMount } from "vue";
import { useRoute, useRouter } from "vue-router";
import _ from "lodash";
import dayjs from "dayjs";
import { message } from "ant-design-vue";
import { ResponseCode, MonitorType, FormatDateKey } from "@/constants";
import { util } from "@/utils";
const props = defineProps({
   data: {
      type: Object,
      default: null
   }
})
const route = useRoute();
const router = useRouter();
const fields = reactive([
   {
      title: $t("MonitorSession.ComputerRoomName"),
      dataIndex: "computerRoomName",
      key: "computerRoomName",
   },
   {
      title: $t("MonitorSession.MonitorTypeLabel"),
      dataIndex: "monitorType",
      key: "monitorType",
   },
   {
      title: $t("MonitorSession.StartDate"),
      dataIndex: "startDate",
      key: "startDate",
   },
   {
      title: $t("MonitorSession.EndDate"),
      dataIndex: "endDate",
      key: "endDate",
   },
   {
      title: $t("MonitorSession.Domain"),
      dataIndex: "domains",
      key: "domains",
   },
]);
const data = ref({});
const loading = reactive({
   isLoadingBeforeMount: false,
});
onBeforeMount(async () => {
   try {
      loading.isLoadingBeforeMount = true;
      data.value = _.cloneDeep(props.data);
      data.value.computerRoomName = data.value?.computerRoom?.name;
      data.value.startDate = data.value.startDate ? dayjs(data.value.startDate).format(FormatDateKey.Default) : "";
      data.value.endDate = data.value.endDate ? dayjs(data.value.endDate).format(FormatDateKey.Default) : "";
      data.value.ownerIdText = data.value?.user?.fullname;
   } catch (error) {
      console.log(error);
   } finally {
      loading.isLoadingBeforeMount = false;
   }
});
</script>
<style lang=""></style>