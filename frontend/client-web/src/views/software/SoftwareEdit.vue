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
               <a-form-item :label="$t('Software.Name')" name="name">
                  <a-input v-model:value="formState.name" />
               </a-form-item>
               <a-form-item :label="$t('Software.Process')" name="process">
                  <a-input v-model:value="formState.process" />
               </a-form-item>
               <a-form-item :label="$t('Software.InstallationFileFolder')" name="installationFileFolder">
                  <a-input v-model:value="formState.installationFileFolder" />
               </a-form-item>
               <a-form-item :label="$t('Software.SoftwareFolder')" name="softwareFolder">
                  <a-input v-model:value="formState.softwareFolder" />
               </a-form-item>
               <a-form-item :label="$t('Software.IsUpdate')" name="isUpdate">
                  <a-checkbox v-model:checked="formState.isUpdate" />
               </a-form-item>
               <a-form-item :label="$t('Software.IsInstall')" name="isInstall">
                  <a-checkbox v-model:checked="formState.isInstall" />
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
   onBeforeRouteUpdate,
   onBeforeRouteLeave,
   useRouter,
} from "vue-router";
import { computerRoomService, softwareService } from "@/api";
import { ResponseCode, FormMode } from "../../constants";
import { message } from "ant-design-vue";
import _ from "lodash";
const route = useRoute();
const router = useRouter();
const formRef = ref();
const labelCol = {
   span: 6,
};
const wrapperCol = {
   span: 12,
};
let formState = ref({
   name: "",
   process: "",
   installationFileFolder: "",
   softwareFolder: "",
   isUpdate: false,
   isInstall: false
});
const loading = reactive({
   isLoadingSave: false,
   isLoadingBeforeMount: false,
});
const isCallCheck = ref(false);
const errorCode = ref(0);
const validateName = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("Software.Name")]));
   } else if (errorCode.value === ResponseCode.ConflicSoftwareName) {
      errorCode.value = 0;
      return Promise.reject($t("Validate.AlreadyExist", [$t("Software.Name")]));
   } else {
      return Promise.resolve();
   }
};
const validateProcess = async (_rule, value) => {
   if (value === "") {
      return Promise.reject($t("Validate.Required", [$t("Software.Process")]));
   } else if (errorCode.value === ResponseCode.ConflicSoftwareProcess) {
      errorCode.value = 0;
      return Promise.reject($t("Validate.AlreadyExist", [$t("Software.Process")]));
   } else {
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
   process: [
      {
         required: true,
         valdator: validateProcess,
         trigger: "change",
      },
   ],
   installationFileFolder: [
      {
         required: true,
         message: $t("Validate.Required", [$t("Software.InstallationFileFolder")]),
         trigger: "change",
      },
   ],
   softwareFolder: [
      {
         required: true,
         message: $t("Validate.Required", [$t("Software.SoftwareFolder")]),
         trigger: "change",
      },
   ],
};

onBeforeMount(async () => {
   if (route.meta.formMode === FormMode.Update) {
      loading.isLoadingBeforeMount = true;
      try {
         let software = await softwareService.getById(route.params.id);
         if (software?.success && software?.data) {
            formState.value = _.cloneDeep(software.data);
         }
      } catch (error) {
         console.log(error);
         switch (error?.Code) {
            case ResponseCode.NotFoundComputerRoom:
               message.error($t("ComputerRoom.Validate.NotFound"));
               break;
            default:
               message.error($t("UnknownError"));
               break;
         }
         router.push({ name: "SoftwareList" });
      } finally {
         loading.isLoadingBeforeMount = false;
      }
   }
});

const onSubmit = async (key) => {
   let passValidate = false;
   try {
      loading.isLoadingSave = true;
      await formRef.value.validate();
      passValidate = true;
      try {
         let rs =
            route.meta.formMode === FormMode.Update
               ? await softwareService.update(formState.value, route.params.id)
               : await softwareService.add(formState.value);
         if (rs?.success && rs?.data) {
            message.success($t("SaveSuccess"));
            if (key == 1) {
               router.push({
                  name: "SoftwareView",
                  params: { id: rs.data },
               });
            } else {
               resetForm();
            }
         }
      } catch (error) {
         console.log(error);
         switch (error?.Code) {
            case ResponseCode.ConflicSoftwareName:
               errorCode.value = error.Code;
               await formRef.value.validateFields("name");
               break;
            case ResponseCode.ConflicSoftwareProcess:
               errorCode.value = error.Code;
               await formRef.value.validateFields("process");
               break;
            default:
               message.error($t("UnknownError"))
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
</script>