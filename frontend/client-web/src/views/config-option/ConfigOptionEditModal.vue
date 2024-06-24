<template>
   <a-modal v-model:open="visible"
      :title="props.formMode == FormMode.Add ? $t('ConfigOption.AddTitle') : $t('ConfigOption.EditTitle')"
      :confirm-loading="confirmLoading" @ok="onOk" @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item name="optionName" :label="$t('ConfigOption.OptionName')">
            <a-input v-model:value="formState.optionName" />
         </a-form-item>
         <a-form-item name="optionValue" :label="$t('ConfigOption.OptionValue')">
            <a-input v-model:value="formState.optionValue" />
         </a-form-item>
         <a-form-item name="isSystem" :label="$t('ConfigOption.IsSystem')">
            <a-checkbox v-model:checked="formState.isSystem"></a-checkbox>
         </a-form-item>
         <a-form-item name="isAgent" :label="$t('ConfigOption.IsAgent')">
            <a-checkbox v-model:checked="formState.isAgent"></a-checkbox>
         </a-form-item>

         <a-form-item name="desc" :label="$t('ConfigOption.Desc')">
            <a-textarea v-model:value="formState.desc" placeholder="Mô tả" :rows="4" />
         </a-form-item>
      </a-form>
   </a-modal>
</template>
<script setup>
import { computerService, configOptionService } from '@/api';
import { ComputerKey, FormMode, ResponseCode } from '@/constants';
import { message } from 'ant-design-vue';
import { reactive, ref, toRaw, onBeforeMount } from 'vue';
import { useRoute } from 'vue-router';
// ========== start state ========== 
const props = defineProps({
   formMode: {
      type: Number,
      default: FormMode.Add
   },
   id: {
      type: String,
      default: null
   }
});
const emit = defineEmits(['toggleShow', 'afterSave'])
const route = useRoute();
const formRef = ref();
const visible = ref(true);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = ref({
   optionName: "",
   optionValue: "",
   isSystem: false,
   isAgent: true,
   desc: "",
});
const validateOptionName = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("ConfigOption.OptionName")]));
   } else if (errorCode.value == ResponseCode.ConflicOptionName) {
      return Promise.reject($t("ConfigOption.Validate.ConflicOptionName"));
   } else if (errorCode.value == ResponseCode.CantEditOptionNameSystem) {
      return Promise.reject($t("ConfigOption.Validate.CantEditOptionNameSystem"));
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
   optionName: [
      {
         required: true,
         validator: validateOptionName,
         trigger: "change",
      },
   ],
   optionValue: [
      {
         required: true,
         message: $t("Validate.Required", [$t("ConfigOption.OptionValue")]),
         trigger: "change",
      },
   ],
};

// ========== end state ==========
// ========== start lifecycle ========== 
onBeforeMount(async () => {
   try {
      if (props.formMode == FormMode.Update) {
         let rs = await configOptionService.getById(props.id);
         if (rs && rs.data) {
            formState.value = rs.data;
         }
      }
   } catch (error) {
      console.log(error);
      message.error($t("UnknownError"));
   }
});
// ========== end lifecycle ==========

const onOk = async () => {
   try {
      confirmLoading.value = true;
      await formRef.value.validate();
      try {
         let rs = props.formMode == FormMode.Add ? await configOptionService.add(formState.value) : await configOptionService.update(formState.value, formState.value.id);
         if (rs && rs.success && rs.data) {
            message.success($t("SaveSuccess"));
            emit('afterSave', {
               id: rs.data,
               formMode: props.formMode
            });
            visible.value = true;
         } else {
            message.error($t("UnknownError"))
         }
      } catch (error) {
         if (error?.Code && ![ResponseCode.Error, ResponseCode.Exception].includes(error.Code)) {
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
   emit('toggleShow', false)
};
</script>
<style scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}
</style>