<template>
   <div class="container-content">
      <template v-if="agentInfo">
         <div class="toolbars flex justify-between py-4">
            <div class="left"></div>
            <div class="right">
               <a-button type="primary" v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="() => edit()">{{
                  $t("Edit") }}</a-button>
            </div>
         </div>
         <div class="content">
            <h2>{{ $t("DetailInfo") }}</h2>
            <a-row :gutter="[16, 24]">
               <template v-for="field in fields" :key="field.dataIndex">
                  <a-col class="gutter-row" :span="6">
                     <label class="font-bold">{{ field.title }}</label>
                  </a-col>

                  <a-col class="gutter-row" :span="18">
                     <template v-if="field.key == 'isUpdate'">
                        <template v-if="agentInfo?.[field.key]">
                           <CheckCircleOutlined />
                        </template>
                        <template v-else>
                           <CloseCircleOutlined />
                        </template>
                     </template>
                     <template v-else-if="field.key === 'fileName'">

                        <div class="flex items-center gap-2">
                           <span>{{ agentInfo?.fileName }}</span>
                           <a-button round @click="dowloadAgentFile()">
                              <DownloadOutlined />
                           </a-button>
                        </div>

                     </template>
                     <template v-else-if="field.key === 'size'">

                        {{ (agentInfo?.[field.key] || 0) / 1024 }} {{ "KB" }}

                     </template>
                     <template v-else-if="field.key === 'createdAt' || field.key === 'updatedAt'">
                        <span v-if="agentInfo?.[field.key]">
                           {{ moment(agentInfo[field.key]).format(FormatDateKey.Default) }}
                        </span>
                     </template>
                     <template v-else>
                        <div class="gutter-box">{{ agentInfo?.[field.key] }}</div>
                     </template>
                  </a-col>
               </template>
            </a-row>
         </div>
      </template>
      <template v-else>
         <div class="p-16 flex justify-center">
            <a-button type="primary" v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="() => add()">{{
               $t("Add") }}</a-button>
         </div>

      </template>

   </div>
   <a-modal v-model:open="visible"
      :title="modalMode == FormMode.Add ? $t('Agent.AddAgentTitle') : $t('Agent.EditAgentTitle')"
      :confirm-loading="confirmLoading" @ok="onOk" @cancel="onCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" layout="vertical" name="form_in_modal">
         <a-form-item :label="$t('Agent.Version')" name="version">
            <a-input v-model:value="formState.version" />
         </a-form-item>
         <a-form-item :label="$t('Agent.IsUpdate')" name="isUpdate">
            <a-checkbox v-model:checked="formState.isUpdate"></a-checkbox>
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
import { FormMode, ResponseCode, UserRole } from "@/constants";
import { ref, reactive, onBeforeMount, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import { agentService } from "@/api"
import { message } from "ant-design-vue";
const router = useRouter();
const route = useRoute();
const agentInfo = ref(null);

const formRef = ref();
const visible = ref(false);
const confirmLoading = ref(false);
const errorCode = ref(0);
const formState = ref({
});
const modalMode = ref(FormMode.Add);
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

const fields = reactive([
   {
      title: $t("Agent.Version"),
      dataIndex: "version",
      key: "version",
   },
   {
      title: $t("Agent.IsUpdate"),
      dataIndex: "isUpdate",
      key: "isUpdate",
   },
   {
      title: $t("Agent.FileName"),
      dataIndex: "fileName",
      key: "fileName",
   },
   {
      title: $t("Agent.Size"),
      dataIndex: "size",
      key: "size",
   },
]);

// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(() => {
   agentInfo.value = route.meta.data;
   if (!agentInfo.value) {
      // chưa có agent mở modal edit để thêm
   }
})
// ========== end lifecycle ==========
const onOk = async () => {
   try {
      confirmLoading.value = true;
      await formRef.value.validate();
      const formData = new FormData();
      formData.append("version", formState.value.version)
      formData.append("isUpdate", formState.value.isUpdate)
      formData.append("filePath", formState.value.fileList[0])
      try {
         let rs = await agentService.upsertAgent(formData);
         if (rs && rs.success && rs.data) {
            visible.value = false;
            message.success($t("SaveSuccess"));
            formState.value = {};
            await reloadData();
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
      errorCode.value = null;
      console.log(error);
   } finally {
      confirmLoading.value = false;
   }
}

const onCancel = () => {
   visible.value = false;
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
const reloadData = async () => {
   try {
      let rs = await agentService.getFirst();
      if (rs && rs.success) {
         agentInfo.value = rs.data
      }
   } catch (error) {
      console.log(error);
   }
}

const edit = () => {
   modalMode.value = FormMode.Update;
   formState.value.version = agentInfo.value.version;
   formState.value.isUpdate = agentInfo.value.isUpdate;
   visible.value = true;
}
const add = () => {
   modalMode.value = FormMode.Add;
   visible.value = true;
}

const dowloadAgentFile = async () => {
   try {
      let blob = await agentService.getFile();
      // Tạo URL tạm thời từ Blob
      const url = URL.createObjectURL(blob);
      // Tạo một liên kết tải về
      const link = document.createElement('a');
      link.href = url;
      link.download = agentInfo.value.fileName; // Tên của file khi tải về
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
</script>
<style lang="">

</style>