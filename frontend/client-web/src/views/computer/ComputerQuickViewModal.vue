<template>

   <a-modal v-model:open="open" :title="$t('Computer.ComputerInfoTitle')" :okText="$t('ViewDetail')"
      :cancelText="$t('Close')" @ok="onOk" @cancel="onCancel">
      <div class="master">
         <a-row :gutter="[16, 24]">
            <template v-for="field in fieldsMaster" :key="field.dataIndex">
               <a-col class="gutter-row" :span="6">
                  <label class="font-bold">{{ field.title }}</label>
               </a-col>

               <a-col class="gutter-row" :span="18">
                  <template v-if="field.key == 'listError'">
                     <span>
                        <a-tag v-for="tag in computerInfo.listError" :key="tag.value" :color="tag.color">
                           {{ tag.label }}
                        </a-tag>
                     </span>
                  </template>
                  <template v-else>
                     <span>{{ computerInfo[field.key] }}</span>
                  </template>
               </a-col>
            </template>
         </a-row>
      </div>
   </a-modal>

</template>
<script setup>
import { computerService } from '@/api';
import computerKey from '@/constants/computerKey';
import { message } from 'ant-design-vue';
import _ from 'lodash';
import { onBeforeMount, reactive, ref, } from 'vue';
import { useRouter } from 'vue-router';
// ========== start state ========== 
const props = defineProps({
   id: {
      type: Number
   }
});

const emit = defineEmits("hideModal")

const router = useRouter();
const loading = reactive({
   spining: false
})
const open = ref(true);
const computerInfo = ref({});
const dataRender = ref({});
const fieldsMaster = [
   {
      title: $t("Computer.ComputerRoomName"),
      dataIndex: "computerRoomName",
      key: "computerRoomName"
   },
   {
      title: $t("Computer.Name"),
      dataIndex: "name",
      key: "name"
   },
   {
      title: $t("Computer.MacAddress"),
      dataIndex: "macAddress",
      key: "macAddress"
   },
   {
      title: $t("Computer.Condition"),
      dataIndex: "listError",
      key: "listError"
   }
]
// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(async () => {
   try {
      loading.spining = true;
      let rs = await computerService.getById(props.id);
      if (rs && rs.success && rs.data) {
         handleDataRender(rs.data);
      }

   } catch (error) {
      console.log(error);
      message.error($t("UnKnowError"));
   }
   finally {
      loading.spining = false;
   }
});
// ========== end lifecycle ==========

// ========== start methods ========== 

const handleDataRender = (item) => {
   item.computerRoomName = item?.computerRoom?.name;
   item.listError = item?.listErrorId?.length > 0 ? item.listErrorId.map(errorId => {
      const { label, color } = handleRenderComputerError(errorId);
      return {
         value: errorId,
         label: label,
         color: color
      }
   }) : [{ value: computerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
   computerInfo.value = _.cloneDeep(item);
}
const onOk = () => {
   router.push({ name: "ComputerView", params: { id: props.id } });
}
const onCancel = () => {
   emit('hideModal', false)
};

const handleRenderComputerError = (errorId) => {
   let label = "", color = "";
   switch (errorId) {
      case computerKey.ComputerError.Perfect:
         label = $t("Computer.ComputerError.Perfect");
         color = "green";
         break;
      case computerKey.ComputerError.Hardware:
         label = $t("Computer.ComputerError.Hardware");
         color = "error";
         break;
      case computerKey.ComputerError.Software:
         label = $t("Computer.ComputerError.Software");
         color = "orange";
         break;
      case computerKey.ComputerError.Network:
         label = $t("Computer.ComputerError.Network");
         color = "blue";
         break;
      case computerKey.ComputerError.OS:
         label = $t("Computer.ComputerError.OS");
         color = "warning";
         break;
      case computerKey.ComputerError.Unknow:
         label = $t("Computer.ComputerError.Unknow");
         color = "red";
         break;
      default:
         break;
   }
   return { label, color }
}
// ========== end methods ==========
</script>