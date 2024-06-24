<template>
   <a-modal v-model:open="visible" :title="$t('Computer.ComputerAddTitle')" :confirm-loading="confirmLoading" @ok="onOk"
      @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item name="name" :label="$t('Computer.Name')">
            <a-input v-model:value="formState.name" />
         </a-form-item>
         <a-form-item name="macAddress" :label="$t('Computer.MacAddress')">
            <a-input v-model:value="formState.macAddress" />
         </a-form-item>
         <a-form-item name="col" :label="$t('Computer.Col')">
            <a-input-number v-model:value="formState.col" :disabled=true />
         </a-form-item>
         <a-form-item name="row" :label="$t('Computer.Row')">
            <a-input-number v-model:value="formState.row" :disabled=true />
         </a-form-item>

         <a-form-item name="modifier" :label="$t('Computer.Condition')" class="collection-create-form_last-form-item">
            <a-select v-model:value="formState.listErrorId" mode="multiple" style="width: 100%"
               placeholder="Tình trạng máy" :options="optionListErrorId"></a-select>
         </a-form-item>
      </a-form>
   </a-modal>
</template>
<script setup>
import { computerService } from '@/api';
import { ComputerKey, ResponseCode } from '@/constants';
import { message } from 'ant-design-vue';
import { reactive, ref, toRaw } from 'vue';
import { useRoute } from 'vue-router';
// ========== start state ========== 
const props = defineProps({
   data: {
      type: Object,
      default: {}
   }
});
const emit = defineEmits(['toggleShowQuickAddComputerModal', 'afterAddComputer'])
const route = useRoute();
const formRef = ref();
const visible = ref(true);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = reactive({
   computerRoomId: route.params.id,
   col: props.data.col,
   row: props.data.row,
   name: "",
   macAddress: "",
   listErrorId: []
});

const optionListErrorId = reactive([
   {
      value: ComputerKey.ComputerError.Hardware,
      label: $t('Computer.ComputerError.Hardware')
   },
   {
      value: ComputerKey.ComputerError.Software,
      label: $t('Computer.ComputerError.Software')
   },
   {
      value: ComputerKey.ComputerError.Network,
      label: $t('Computer.ComputerError.Network')
   },
   {
      value: ComputerKey.ComputerError.OS,
      label: $t('Computer.ComputerError.OS')
   },
   {
      value: ComputerKey.ComputerError.Unknow,
      label: $t('Computer.ComputerError.Unknow')
   },
])
const validateName = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("Computer.Name")]));
   } else if (errorCode.value == ResponseCode.ConflicNameComputer) {
      return Promise.reject($t("Computer.Validate.ConflicNameComputer"));
   }
   else {
      return Promise.resolve();
   }
};
const validateMacAddress = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("Computer.MacAddress")]));
   } else if (errorCode.value == ResponseCode.ConflicMacAddress) {
      return Promise.reject($t("Computer.Validate.ConflicMacAddressComputer"));
   }
   else {
      return Promise.resolve();
   }
};
const rules = {
   name: [
      {
         required: true,
         validator: validateName,
         trigger: "change",
      },
   ],
   macAddress: [
      {
         required: true,
         validator: validateMacAddress,
         trigger: "change",
      },
   ],
};


// ========== end state ==========
const onOk = async () => {
   try {
      confirmLoading.value = true;
      errorCode.value = 0;
      await formRef.value.validate();
      try {
         let rs = await computerService.add(formState);
         if (rs && rs.success && rs.data) {
            formRef.value.id = rs.data;
            emit('afterAddComputer', formRef.value);
            visible.value = true;
         } else {
            message.error($t("UnknownError"))
         }
      } catch (error) {
         if (error?.Code) {
            errorCode.value = error.Code;
            await formRef.value.validate();
         } else {
            message.error($t("UnknownError"))
         }
      }
   } catch (error) {
      errorCode.value = 0;
      console.log(error);
   } finally {
      confirmLoading.value = false;
   }
}

const onCancel = () => {
   emit('toggleShowQuickAddComputerModal', false)
};
</script>
<style scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}
</style>