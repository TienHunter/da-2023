<template>
   <a-modal v-model:open="visible" :title="$t('auth.ChangePassword')" :confirm-loading="confirmLoading" @ok="onOk"
      @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item name="oldPassword" :label="$t('auth.OldPassword')">
            <a-input-password v-model:value="formState.oldPassword" />
         </a-form-item>
         <a-form-item name="newPassword" :label="$t('auth.NewPassword')">
            <a-input-password v-model:value="formState.newPassword" />
         </a-form-item>
         <a-form-item name="confirmPassowrd" :label="$t('auth.ConfirmPasswordHint')">
            <a-input-password v-model:value="formState.confirmPassowrd" />
         </a-form-item>
      </a-form>
   </a-modal>
</template>
<script setup>
import { computerService, userService } from '@/api';
import { ComputerKey, ResponseCode } from '@/constants';
import { message } from 'ant-design-vue';
import { reactive, ref, toRaw, defineProps, defineEmits } from 'vue';
import { useRoute } from 'vue-router';
// ========== start state ========== 
const props = defineProps({
});
const emit = defineEmits(['toggleShowModal', 'afterSave'])
const route = useRoute();
const formRef = ref();
const visible = ref(true);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = reactive({
   oldPassword: "",
   newPassword: "",
   confirmPassowrd: "",
});

const validateOldPassword = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("auth.OldPassword")]));
   } else if (errorCode.value == ResponseCode.ConflicNameComputer) {
      return Promise.reject($t("Computer.Validate.ConflicNameComputer"));
   }
   else {
      return Promise.resolve();
   }
};
const validateNewPawword = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("auth.NewPassword")]));
   } else if (formState.confirmPassowrd !== "") {
      formRef.value.validateFields('confirmPassowrd');
      return Promise.resolve();
   }
   else {
      return Promise.resolve();
   }
};

const validateConfirmPassword = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("auth.ConfirmPasswordHint")]));
   } else if (value !== formState.newPassword) {
      return Promise.reject($t("auth.WrongConfirmPassword"));
   }
   else {
      return Promise.resolve();
   }
};

const rules = {
   oldPassword: [
      {
         required: true,
         validator: validateOldPassword,
         trigger: "change",
      },
   ],
   newPassword: [
      {
         required: true,
         validator: validateNewPawword,
         trigger: "change",
      },
   ],
   confirmPassowrd: [
      {
         required: true,
         validator: validateConfirmPassword,
         trigger: "change",
      },
   ],
};


// ========== end state ==========
const onOk = async () => {
   try {
      confirmLoading.value = true;
      await formRef.value.validate();
      try {
         let rs = await userService.changePassword(formState);
         if (rs && rs.success && rs.data) {
            visible.value = true;
            emit('afterSave');
         } else {
            message.error($t("UnknownError"))
         }
      } catch (error) {
         console.log(error);
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
   emit('toggleShowModal', false)
};
</script>
<style scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}
</style>