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
const route = useRoute();
const router = useRouter();
const formRef = ref();
const labelCol = {
   span: 3,
};
const wrapperCol = {
   span: 6,
};
let formState = ref({
   name: "",
   isUpdate: true,
   isInstall: true
});
const loading = reactive({
   isLoadingSave: false,
   isLoadingBeforeMount: false,
});
const isCallCheck = ref(false);
const rules = {
   name: [
      {
         required: true,
         message: $t("Validate.Required", [$t("Software.Name")]),
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
            formState.value = reactive(computer.data);
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
</script>