<template>
   <a-modal v-model:open="visible" :title="$t('MonitorSession.QuickAddMonitorSessionTitle')"
      :confirm-loading="confirmLoading" @ok="onOk" @cancel="onCancel">
      <div class="container flex">
         <a-form ref="formRef" :model="formState" :rules="rules" v-bind="formItemLayout">
            <a-form-item :label="$t('MonitorSession.ComputerRoomName')" name="computerRoomId">
               <a-select v-model:value="formState.computerRoomId" show-search placeholder="input search text"
                  :field-names="{ label: 'name', value: 'id' }" :options="dataComputerRooms" :disabled="true" />
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
         <a-divider type="vertical" style="width: 2px;height: auto; background-color: #7cb305" />
         <div class="list-session flex-shrink-0">
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
            <h3>fdsf</h3>
         </div>
      </div>

   </a-modal>
</template>
<script setup>
import { computerService, fileService, monitorSessionService } from '@/api';
import { ComputerKey, FormatDateKey, LocalStorageKey, MonitorType, ResponseCode } from '@/constants';
import { message } from 'ant-design-vue';
import moment from 'moment';
import dayjs from 'dayjs';
import { reactive, ref, toRaw, defineProps, defineEmits, watch } from 'vue';
import { useRoute } from 'vue-router';
import { localStore } from '@/utils';
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
   computerRoomId: props.data.computerRoom.id,
   monitorType: MonitorType.Practive,
   date: dayjs(),
   timeRange: [],
   domains: [],
   ownerId: localStore.getItem(LocalStorageKey.userInfor)?.id,
});
const formItemLayout = {
   labelCol: {
      xs: {
         span: 24,
      },
      sm: {
         span: 10,
      },
   },
   wrapperCol: {
      xs: {
         span: 24,
      },
      sm: {
         span: 14,
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
         span: 14,
         offset: 10,
      },
   },
};
const dataComputerRooms = ref([{
   id: props.data.computerRoom.id,
   name: props.data.computerRoom.name
}]);
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

const validateTimeRange = async (_rule, value) => {
   if (!formState.value?.timeRange.length) {
      return Promise.reject($t("Validate.Required", [$t("MonitorSession.Session")]));
   } else {
      return Promise.resolve();
   }
}
const rules = {
   computerRoomId: [
      {
         required: true,
         message: $t("Validate.Required", [$t("MonitorSession.Date")]),
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

// ========== end state ==========

// ========== start lifecycle ========== 

watch(() => formState.value.date, () => {
   console.log(formState.value.date)
});
// ========== end lifecycle ==========
const resetForm = () => {
   formRef.value.resetFields();
};
const removeDomain = (item, index) => {
   if (formState.value.domains.length > index && index >= 0) {
      formState.value.domains.splice(index, 1);
   }
};
const addDomain = () => {
   formState.value.domains.push("");
};
const disabledDate = current => {
   // Can not select days before today and today
   return current && current < dayjs().endOf('day');
};
const onOk = async () => {
   try {
      confirmLoading.value = true;
      await formRef.value.validate();
      let dateTmp = dayjs(formState.value.date);

      formState.value.startDate = dateTmp.set('hour', formState.value.timeRange[0].get('hour')).set('minute', formState.value.timeRange[0].get('minute')).set('second', 0).format();
      formState.value.endDate = dateTmp.set('hour', formState.value.timeRange[1].get('hour')).set('minute', formState.value.timeRange[1].get('minute')).set('second', 0).format();
      try {
         let rs = await monitorSessionService.add(formState.value);
         if (rs && rs.success && rs.data) {
            formRef.value.id = rs.data;
            emit('afterSave', formRef.value);
            visible.value = true;
         } else {
            message.error($t("UnKnownError"))
         }
      } catch (error) {
         console.log(error);
         if (error?.Code) {
            errorCode.value = error.Code;
            await formRef.value.validate();
         } else {
            message.error($t("UnKnownError"))
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
<style class="scss" scoped>
.collection-create-form_last-form-item {
   margin-bottom: 0;
}

.container {
   position: relative;

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