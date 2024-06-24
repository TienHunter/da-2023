<template>
   <a-modal v-model:open="visible"
      :title="props.formMode == FormMode.Update ? $t('Student.EditTitle') : $t('Student.AddTitle')"
      :confirm-loading="confirmLoading" width="400px" @ok="onOk" @cancel="onCancel">
      <div class="container flex">
         <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical">
            <a-form-item name="studentCode" :label="$t('Student.StudentCode')">
               <a-input v-model:value="formState.studentCode" :disabled="props.formMode == FormMode.Update" />
            </a-form-item>
            <a-form-item name="studentName" :label="$t('Student.StudentName')">
               <a-input v-model:value="formState.studentName" />
            </a-form-item>
         </a-form>
      </div>

   </a-modal>
</template>
<script setup>
import { monitorSessionService, studentService } from '@/api';
import { FormMode, ResponseCode } from '@/constants';
import { message } from 'ant-design-vue';
import moment from 'moment';
import dayjs from 'dayjs';
import { reactive, ref, toRaw, watch, onBeforeMount } from 'vue';
import { useRoute } from 'vue-router';
import { localStore } from '@/utils';
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
const emit = defineEmits(['toggleShowModal', 'afterSave']);
const route = useRoute();
const formRef = ref();
const visible = ref(true);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = ref({
   studentCode: "",
   studentName: "",
});
const validateStudentCode = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("Student.StudentCode")]));
   } else if (errorCode.value == ResponseCode.ConflicStudentCode) {
      return Promise.reject($t("Student.Validate.ConflicStudentCode"));
   }
   else {
      return Promise.resolve();
   }
};
const rules = {
   studentCode: [
      {
         required: true,
         validator: validateStudentCode,
         trigger: "change",
      },
   ],
   studentName: [
      {
         required: true,
         message: $t("Validate.Required", [$t("Student.StudentName")]),
         trigger: "change",
      },
   ],
};

// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(async () => {
   try {
      if (props.formMode == FormMode.Update) {
         let rs = await studentService.getById(props.id);
         if (rs && rs.data) {
            formState.value = rs.data
         } else {
            message.error($t("NotFound"));
            emit("toggleShowModal", false);
         }
      }
   } catch (error) {
      message.error($t("UnknownError"));
      emit("toggleShowModal", false);
      console.log(error);
   }

})
// ========== end lifecycle ==========
const onOk = async () => {
   try {
      confirmLoading.value = true;
      errorCode.value = 0;
      await formRef.value.validate();
      try {
         let rs = props.formMode == FormMode.Update ? await studentService.update(formState.value, formState.value.id) : await studentService.add(formState.value);
         if (rs && rs.success && rs.data) {
            formRef.value.id = rs.data;
            message.success($t("SaveSuccess"))
            emit('afterSave', {
               data: rs.data
            });
            visible.value = true;
         } else {
            message.error($t("UnknownError"))
         }
      } catch (error) {
         console.log(error);
         if (error?.Code && ![ResponseCode.Error, ResponseCode.Exception].includes(error.Code)) {
            errorCode.value = error.Code;
            await formRef.value.validate();
         } else {
            message.error($t("UnknownError"))
         }
      }
   } catch (error) {
      errorCode.value = null;
      console.log(error);
   } finally {
      confirmLoading.value = false;
   }
}

const onCancel = () => {
   emit('toggleShowModal', false)
};
</script>
<style class="scss" scoped></style>