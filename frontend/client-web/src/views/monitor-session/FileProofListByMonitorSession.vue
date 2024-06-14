<template>
   <div class="container-content">
      <div class="tool-bars flex justify-between py-4 sticky top-0 -mx-4 px-4 ">
         <div class="tool-bars__left">
         </div>
         <div class="tool-bars__right">
            <a-input v-model:value="searchValue" placeholder="Nhập từ khóa" allow-clear style="width: 200px" />
         </div>
      </div>
      <div class="content pt-4">

         <a-table :columns="columnFiles" :data-source="dataClones" :pagination="false" :scroll="scrollConfig"
            :loading="loading.loadingTable">
            <template #bodyCell="{ column, record }">
               <template v-if="column.key === 'studentCode'">
                  {{ record?.student?.studentCode }}
               </template>
               <template v-else-if="column.key === 'studentName'">
                  {{ record?.student?.studentName }}
               </template>
               <template v-else-if="column.key === 'computerName'">
                  {{ record?.computer?.name }}
               </template>
               <template v-else-if="column.key === 'createdAt'">
                  <span v-if="record[column.key]">
                     {{ moment(record[column.key]).format(FormatDateKey.Default) }}
                  </span>
               </template>
               <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                     <a-button round :disabled="!record?.fileName" @click="dowliadFile(record?.fileName)">
                        <template #icon>
                           <DownloadOutlined />
                        </template>
                     </a-button>
                  </div>
               </template>
            </template>
            <template #footer> {{ showTotal }} </template>
         </a-table>
      </div>
   </div>
</template>
<script setup>
import { onBeforeMount, ref, reactive, computed, h, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import _ from "lodash";
import moment from "moment";
import { Modal, message } from "ant-design-vue";
import { ActionTypeSocket, FormatDateKey } from "@/constants";
import { fileProofService } from "@/api";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
// ========== start state ========== 
const props = defineProps({
   data: {
      type: Object,
      default: null
   },
   isHasSession: {
      type: Boolean,
      default: false
   }
})
const router = useRouter();
const route = useRoute();
const loading = reactive({
   loadingTable: false,
});
const searchValue = ref("");
const columnFiles = computed(() => {
   return [
      {
         title: $t("FileProof.StudentCode"),
         dataIndex: "studentCode",
         key: "studentCode",
         width: "60px",
         fixed: "left",
      },
      {
         title: $t("FileProof.StudentName"),
         dataIndex: "studentName",
         key: "studentName",
         width: "100px",
         ellipsis: true,
      },
      {
         title: $t("FileProof.ComputerName"),
         dataIndex: "computerName",
         key: "computerName",
         width: "60px",
      },
      {
         title: $t("FileProof.CreatedAt"),
         dataIndex: "createdAt",
         key: "createdAt",
         width: "100px",
      },
      {
         title: $t("Action"),
         dataIndex: "operation",
         key: "operation",
         width: "40px",
         fixed: "right",
      },
   ]
})
const dataFiles = ref([]);
const dataClones = ref([]);
const showTotal = computed(
   () => `Total ${dataClones.value?.length || 0} items`
);
const scrollConfig = ref({ y: "calc(100vh - 340px)" });
// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(async () => {
   try {
      await loadListData();
   } catch (error) {
      console.log(error);
      message.error($t("UnknownError"))
   }
   finally {

   }
})

onMounted(() => {
   onUseSocket();
}),
   onBeforeMount(async () => {
      // lấy danh sách
      try {
         await loadListData();
      } catch (error) {
         console.log(error);
         message.error($t("UnknownError"));
      }
   });

watch(searchValue, () => {
   debounceSearch()
})

// ========== start methods ========== 
const onUseSocket = () => {

   // còn phiêm thì mới mở socket
   if (props.isHasSession) {
      // lắng nghe socket
      const conn = new HubConnectionBuilder().withUrl(`${process.env.VUE_APP_API_BASE_URL}/ws`).configureLogging(LogLevel.Information).withAutomaticReconnect().build();
      conn.start()
         .then(() => {
            console.log("SignalR connection established:");
            // kết nối theo id session
            conn.invoke("Connect", props.data.id)
         })
         .catch((error) => {
            console.error("Error establishing SignalR connection:", error);
         });
      conn.on("ReceviceMessageConnect", (message) => {
         console.log("ReceviceMessageConnect:", message);
      })
      conn.on("ReceviceMessage", (res) => {
         try {
            switch (res.actionType) {
               case ActionTypeSocket.ADD_FILE_PROOF:
                  addFileProofBySocket(res.message)
                  break;
               default:
                  break;
            }

         } catch (error) {
            console.log(error);
         }
      })

   }

}
/**
 * fetch danh sách file theo id phần mềm
 */
const loadListData = async () => {
   try {
      loading.loadingTable = true;
      const rs = await fileProofService.getListByMonitorSessionId(route.params.id);
      if (rs && rs.success && rs.data) {
         dataFiles.value = rs.data;
         dataClones.value = _.cloneDeep(rs.data);
      }
   } catch (error) {
      console.log(error);
      message.error($t("UnknownError"));
   }
   finally {
      loading.loadingTable = false;
   }
}

const addFileProofBySocket = (item) => {
   if (item) {
      dataFiles.value.unshift(item);
      filterData(item);
   }
}
const debounceSearch = _.debounce(() => {
   filterData();
}, 600);
const filterData = (item) => {
   if (item) {
      if (checkFilter(item)) {
         dataClones.value.unshift(item);
      }
   } else {
      dataClones.value = _.cloneDeep(dataFiles.value.filter(i => checkFilter(i)));
   }

}
const checkFilter = (item) => {
   if (item) {
      return (item?.student?.studentName?.includes(searchValue.value) || item?.student?.studentCode?.includes(searchValue.value) || item?.computer?.name?.includes(searchValue.value))
   }
   return false;
}

/**
 * tải file
 */
const dowliadFile = async (fileName) => {
   if (fileName) {
      try {
         let blob = await fileProofService.getFileByFilename(fileName);
         // Tạo URL tạm thời từ Blob
         const url = URL.createObjectURL(blob);
         // Tạo một liên kết tải về
         const link = document.createElement('a');
         link.href = url;
         link.download = fileName; // Tên của file khi tải về
         // Thêm liên kết vào body và nhấp tự động
         document.body.appendChild(link);
         link.click();
         // Xóa URL tạm thời sau khi đã sử dụng
         URL.revokeObjectURL(url);
      } catch (error) {
         console.log(error);
         message.error("UnknownError");
      }
   }

}
// ========== end methods ==========


// ========== end lifecycle ==========
</script>
<style lang="scss" scoped>
.container-content {
   height: 100%;
   overflow-y: auto;

   .tool-bars {
      z-index: 1;
      background-color: #f5f5f5;
      border-bottom: 1px solid rgba(5, 5, 5, 0.06);
   }


}
</style>