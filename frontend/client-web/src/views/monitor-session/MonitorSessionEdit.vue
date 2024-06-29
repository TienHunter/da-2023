<template>
   <a-spin tip="Loading..." :spinning="loading.isLoadingBeforeMount">
      <div class="container-content">
         <div class="toolbars flex justify-between p-4 rounded">
            <div class="toolbars-left">
               <router-link :to="{ name: 'ComputerRoomList' }">
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
            <a-form ref="formRef" :model="formState" :rules="rules" v-bind="formItemLayout">
               <a-form-item :label="$t('MonitorSession.ComputerRoomName')" name="computerRoomId">
                  <a-select v-model:value="formState.computerRoomId" show-search placeholder="Nhập tên phòng máy"
                     :field-names="{ label: 'name', value: 'id' }" :filter-option="filterOption"
                     :options="listComputerRoom" />
               </a-form-item>
               <a-form-item :label="$t('MonitorSession.MonitorTypeLabel')" name="monitorType">
                  <a-select v-model:value="formState.monitorType" show-search placeholder="Chọn loại giám sát"
                     :options="dataMonitorTypes" />
               </a-form-item>
               <a-form-item :label="$t('MonitorSession.Date')" name="date">
                  <a-date-picker v-model:value="formState.date" :format="FormatDateKey.DateDefault"
                     :disabledDate="disabledDate" />
               </a-form-item>
               <a-form-item :label="$t('MonitorSession.Session')" name="timeRange">
                  <a-time-range-picker v-model:value="formState.timeRange" format="HH:mm" />
               </a-form-item>
               <a-form-item v-for="(domain, index) in formState.domains" :key="index"
                  v-bind="index > 0 ? formItemLayoutWithOutLabel : {}"
                  :label="index === 0 ? $t('MonitorSession.Domain') : ''" :name="['domains', index]">
                  <a-input v-model:value="formState.domains[index]" style="width: 60%; margin-right: 8px" />
                  <MinusCircleOutlined v-if="formState.domains.length > 0" class="dynamic-delete-button"
                     @click="removeDomain(domain, index)" />
               </a-form-item>
               <a-form-item v-bind="formItemLayoutWithOutLabel">
                  <a-button type="dashed" @click="addDomain">
                     <PlusOutlined />
                     {{ $t("MonitorSession.AddDomain") }}
                  </a-button>
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
import { computerRoomService, monitorSessionService } from "@/api";
import { ResponseCode, FormMode, MonitorType, LocalStorageKey, FormatDateKey } from "../../constants";
import { message } from "ant-design-vue";
import dayjs from "dayjs";
import { localStore } from "@/utils";
const route = useRoute();
const router = useRouter();
const formRef = ref();
const formItemLayout = {
   labelCol: {
      xs: {
         span: 24,
      },
      sm: {
         span: 6,
      },
   },
   wrapperCol: {
      xs: {
         span: 24,
      },
      sm: {
         span: 18,
      },
   },
};
const formItemLayoutWithOutLabel = {
   wrapperCol: {
      xs: {
         span: 24,
         offset: 0,
      },
      sm: {
         span: 18,
         offset: 6,
      },
   },
};
let formState = ref({
   computerRoomId: "",
   monitorType: MonitorType.Practive,
   date: dayjs(),
   timeRange: [],
   domains: [],
   ownerId: localStore.getItem(LocalStorageKey.userInfor)?.id,
});
const loading = reactive({
   isLoadingSave: false,
   isLoadingBeforeMount: false,
});
const isCallCheck = ref(false);
const filterOption = (input, option) => {
   return option.name.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const listComputerRoom = ref([]);
const dataMonitorTypes = ref([
   {
      value: MonitorType.Practive,
      label: $t("MonitorSession.MonitorType.Practive")
   },
   {
      value: MonitorType.Exam,
      label: $t("MonitorSession.MonitorType.Exam")
   }
])
const errorCode = ref(0);
const validateTimeRange = async (_rule, value) => {
   if (!formState.value?.timeRange?.length) {
      return Promise.reject($t("Validate.Required", [$t("MonitorSession.SessionTime")]));
   } else if (errorCode.value == ResponseCode.ConflicMonitorSessionTime) {
      return Promise.reject($t("MonitorSession.Validate.ConflicMonitorSessionTime"));
   }
   else {
      return Promise.resolve();
   }
}

const rules = {
   computerRoomId: [
      {
         required: true,
         message: $t("Validate.Required", [$t("MonitorSession.ComputerRoomName")]),
         trigger: "change",
      },
   ],
   date: [
      {
         required: true,
         message: $t("Validate.Required", [$t("MonitorSession.MonitorTypeLabel")]),
         trigger: "change",
      },
   ],
   monitorType: [
      {
         required: true,
         message: $t("Validate.Required", [$t("MonitorSession.MonitorTypeLabel")]),
         trigger: "change",
      },
   ],
   timeRange: [
      {
         required: true,
         validator: validateTimeRange,
         trigger: "change",
      },
   ]
};

onBeforeMount(async () => {
   loading.isLoadingBeforeMount = true;
   try {
      await getListComputerRoom();
      if (route.meta.formMode === FormMode.Update) {
         let ms = await monitorSessionService.getById(route.params.id);
         if (ms?.success && ms?.data) {
            formState.value = ms.data;
         }
      }
   } catch (error) {
      switch (error?.Code) {
         case ResponseCode.NotFoundComputerRoom:
            message.error($t("ComputerRoom.Validate.NotFound"));
            break;
         default:
            message.error($t("UnknownError"));
            break;
      }
      router.push({ name: "MonitorSessionList" });
   } finally {
      loading.isLoadingBeforeMount = false;
   }

});

const getListComputerRoom = async () => {
   try {
      let rs = await computerRoomService.getList({});
      if (rs && rs.data && rs.data.list) {
         listComputerRoom.value = rs.data.list
      }
   } catch (error) {
      console.log(error);
   }
}
const disabledDate = current => {
   // Can not select days before today and today
   return current && current < dayjs().endOf('day');
};
const removeDomain = (item, index) => {
   if (formState.value.domains.length > index && index >= 0) {
      formState.value.domains.splice(index, 1);
   }
};
const addDomain = () => {
   formState.value.domains.push("");
};
const onSubmit = async (key) => {
   let passValidate = false;
   try {
      loading.isLoadingSave = true;
      await formRef.value.validate();
      let dateTmp = dayjs(formState.value.date);

      formState.value.startDate = dateTmp.set('hour', formState.value.timeRange[0].get('hour')).set('minute', formState.value.timeRange[0].get('minute')).set('second', 0).format();
      formState.value.endDate = dateTmp.set('hour', formState.value.timeRange[1].get('hour')).set('minute', formState.value.timeRange[1].get('minute')).set('second', 0).format();
      try {
         let rs =
            route.meta.formMode === FormMode.Update
               ? await monitorSessionService.update(formState.value, route.params.id)
               : await monitorSessionService.add(formState.value);
         if (rs?.success && rs?.data) {
            message.success($t("SaveSuccess"));
            if (key == 1) {
               router.push({
                  name: "MonitorSessionView",
                  params: { id: rs.data },
               });
            } else {
               resetForm();
            }
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
   } finally {
      loading.isLoadingSave = false;
   }
};
const resetForm = () => {
   formRef.value.resetFields();
};
</script>
<style class="scss" scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}

.container-content {
   position: relative;
   height: calc(100vh - 48px);

   .content {
      height: calc(100% - 48px);
      overflow: auto;
   }

   .list-session {
      background-color: white;
      padding: 24px;
      overflow: auto
   }

   .dynamic-delete-button {
      cursor: pointer;
      position: relative;
      top: 4px;
      font-size: 24px;
      color: #999;
      transition: all 0.3s;
   }

   .dynamic-delete-button:hover {
      color: #777;
   }

   .dynamic-delete-button[disabled] {
      cursor: not-allowed;
      opacity: 0.5;
   }
}
</style>
