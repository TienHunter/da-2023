<template>
   <a-modal v-model:open="visible" :title="$t('File.AddFileTitle')" :confirm-loading="confirmLoading" @ok="onOk"
      @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item :label="$t('File.SoftwareName')" name="softwareId">
            <a-select v-model:value="formState.softwareId" show-search placeholder="input search text"
               :field-names="{ label: 'name', value: 'id' }" :options="dataSoftware" :disabled="true" />
         </a-form-item>
         <a-form-item :label="$t('File.Version')" name="version">
            <a-input v-model:value="formState.version" />
         </a-form-item>

         <a-form-item :label="$t('File.FileSource')" name="fileList">
            <a-upload :file-list="formState.fileList" :before-upload="beforeUpload" :max-count="1"
               @remove="handleRemove">
               <a-button>
                  <upload-outlined></upload-outlined>
                  Select File
               </a-button>
            </a-upload>
         </a-form-item>
      </a-form>
   </a-modal>
</template>
<script setup>
import { computerService, fileService } from '@/api';
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
const emit = defineEmits(['toggleShowModal', 'afterSave']);
const route = useRoute();
const formRef = ref();
const visible = ref(true);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = ref({
   softwareId: props.data.softwareId,
});
const dataSoftware = ref([{
   id: props.data.softwareId,
   name: props.data.softwareName
}]);
const validateFile = async (_rule, value) => {
   if (!formState.value?.fileList?.length) {
      return Promise.reject($t("File.Validate.FileRequired"));
   } else {
      return Promise.resolve();
   }
};
const rules = {
   softwareId: [
      {
         required: true,
         message: $t("Validate.Required", [$t("Software.Name")]),
         trigger: "change",
      },
   ],
   version: [
      {
         required: true,
         message: $t("Validate.Required", [$t("File.Version")]),
         trigger: "change",
      },
   ],
   fileList: [
      {
         required: true,
         validator: validateFile,
         trigger: "change",
      }
   ]
};

// ========== end state ==========
const onOk = async () => {
   try {
      confirmLoading.value = true;
      await formRef.value.validate();
      const formData = new FormData();
      formData.append("softwareId", formState.value.softwareId)
      formData.append("version", formState.value.version)
      formData.append("filePath", formState.value.fileList[0])
      try {
         let rs = await fileService.uploadFile(formData);
         if (rs && rs.success && rs.data) {
            formRef.value.id = rs.data;
            visible.value = true;
            message.success($t("SaveSuccess"));
            emit('afterSave', formRef.value);
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
      errorCode.value = null;
      console.log(error);
   } finally {
      confirmLoading.value = false;
   }
}

const onCancel = () => {
   emit('toggleShowModal', false)
};
const handleRemove = file => {
   const index = formState.value.fileList.indexOf(file);
   const newFileList = formState.value.fileList.slice();
   newFileList.splice(index, 1);
   formState.value.fileList = newFileList;
};
const beforeUpload = file => {
   formState.value.fileList = [file];
   return false;
};
</script>
<style scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}
</style>