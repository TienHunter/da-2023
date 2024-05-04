<template>
   <a-spin tip="Loading..." :spinning="loading.isLoadingBeforeMount">
      <div class="container-content">
         <div class="toolbars flex justify-between p-4 rounded">
            <div class="toolbars-left">
               <router-link :to="{ name: 'SoftwareList' }">
                  <a-button shape="circle">
                     <template #icon>
                        <ArrowLeftOutlined />
                     </template>
                  </a-button>
               </router-link>
            </div>
            <div class="toolbars-right flex gap-2">
               <a-button type="primary" ghost @click="onSubmit(1)" :loading="loading.isLoadingSave">{{ $t("Save")
                  }}</a-button>
               <a-button type="primary" @click="onSubmit(2)">{{
                  $t("SaveAndAdd")
               }}</a-button>
               <a-button @click="resetForm">{{ $t("Cancel") }}</a-button>
            </div>
         </div>
         <div class="content">
            <a-form ref="formRef" :model="formState" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
               <a-form-item :label="$t('File.SoftwareName')" name="softwareId">
                  <a-select v-model:value="formState.softwareId" show-search placeholder="input search text"
                     :field-names="{ label: 'name', value: 'id' }" :filter-option="filterOption" :options="dataSoftware"
                     @change="handleChange" />
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
         </div>
      </div>
   </a-spin>
</template>
<script setup>
import { reactive, ref, onBeforeMount } from "vue";
import {
   useRoute,
   useRouter,
} from "vue-router";
import { computerRoomService, softwareService, fileService } from "@/api";
import { ResponseCode, FormMode } from "../../constants";
import { message } from "ant-design-vue";
import _ from "lodash";
const route = useRoute();
const router = useRouter();
const formRef = ref();
const labelCol = {
   span: 3,
};
const wrapperCol = {
   span: 6,
};
const formState = ref({
});
const loading = reactive({
   isLoadingSave: false,
   isLoadingBeforeMount: false,
});
const dataSoftware = ref([]);
const filterOption = (input, option) => {
   return option.name.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const isCallCheck = ref(false);

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
         message: $t("Validate.Required", [$t("Software.Version")]),
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

onBeforeMount(async () => {
   if (route.meta.formMode === FormMode.Update) {
      loading.isLoadingBeforeMount = true;
      try {
         let fileData = await fileService.getById(route.params.id);
         if (software?.success && software?.data) {
            formState.value = _.cloneDeep(fileData.data);
         }
      } catch (error) {
         switch (error?.Code) {
            case ResponseCode.NotFoundComputerRoom:
               message.error($t("ComputerRoom.Validate.NotFound"));
               break;
            default:
               message.error($t("UnKnowError"));
               break;
         }
         router.push({ name: "SoftwareList" });
      } finally {
         loading.isLoadingBeforeMount = false;
      }
   }

   try {
      let softwareList = await softwareService.getList();
      if (softwareList && softwareList.success && softwareList.data && softwareList.data.list) {
         dataSoftware.value = softwareList.data.list;
      }
   } catch (error) {
      console.log(error);
      message.error($t("UnKnowError"));
   }
});

const onSubmit = async (key) => {
   try {
      loading.isLoadingSave = true;
      await formRef.value.validate();
      const formData = new FormData();
      formData.append("softwareId", formState.value.softwareId)
      formData.append("version", formState.value.version)
      formData.append("filePath", formState.value.fileList[0])
      try {
         let rs =
            route.meta.formMode === FormMode.Update
               ? await fileService.update(formData, route.params.id)
               : await fileService.uploadFile(formData);
         if (rs?.success && rs?.data) {
            message.success($t("SaveSuccess"));
            if (key == 1) {
               router.push({
                  name: "FileView",
                  params: { id: rs.data },
               });
            } else {
               resetForm();
            }
         }
      } catch (error) {
         console.log(error);
         switch (error?.Code) {
            default:
               break;
         }
      }
   } catch (error) {
   } finally {
      loading.isLoadingSave = false;
   }
};
const resetForm = () => {
   formRef.value.resetFields();
};
const handleChange = (value) => {
   console.log("select:", value);
}
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
</script>