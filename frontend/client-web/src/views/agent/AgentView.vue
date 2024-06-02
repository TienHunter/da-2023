<template>
   <div class="container-content">
      agent view
   </div>
   <a-modal v-model:open="visible" :title="$t('File.AddFileTitle')" :confirm-loading="confirmLoading" @ok="onOk"
      @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item :label="$t('Agent.Version')" name="version">
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
import { FormMode } from "@/constants";
import { ref, reactive, onBeforeMount, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
const router = useRouter();
const route = useRoute();
const agentInfo = ref(null);

const formRef = ref();
const visible = ref(false);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = ref({
   softwareId: props.data.softwareId,
});
const modalMode = FormMode.Add;
const validateFile = async (_rule, value) => {
   if (!formState.value?.fileList?.length) {
      return Promise.reject($t("File.Validate.FileRequired"));
   } else {
      return Promise.resolve();
   }
};
const rules = {
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
   // try {
   //    confirmLoading.value = true;
   //    await formRef.value.validate();
   //    const formData = new FormData();
   //    formData.append("version", formState.value.version)
   //    formData.append("filePath", formState.value.fileList[0])
   //    try {
   //       let rs = await fileService.uploadFile(formData);
   //       if (rs && rs.success && rs.data) {
   //          formRef.value.id = rs.data;
   //          visible.value = true;
   //          message.success($t("SaveSuccess"));
   //          emit('afterSave', formRef.value);
   //       } else {
   //          message.error($t("UnknownError"))
   //       }
   //    } catch (error) {
   //       if (error?.Code) {
   //          errorCode.value = error.Code;
   //          await formRef.value.validate();
   //       } else {
   //          message.error($t("UnknownError"))
   //       }
   //    }
   // } catch (error) {
   //    errorCode.value = null;
   //    console.log(error);
   // } finally {
   //    confirmLoading.value = false;
   // }
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
   formState.value.fileList = [...(formState.value.fileList || []), file];
   return false;
};
onBeforeMount(() => {
   agentInfo.value = route.meta.data;
   if (!agentInfo.value) {
      // chưa có agent mở modal edit để thêm
   }
})
</script>
<style lang="">

</style>