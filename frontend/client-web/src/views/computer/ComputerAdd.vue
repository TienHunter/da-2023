<template>
  <a-spin tip="Loading..." :spinning="loading.isLoadingBeforeMount">
    <div class="container-content">
      <div class="toolbars flex justify-between p-4 rounded">
        <div class="toolbars-left">
          <router-link :to="{ name: 'ComputerList' }">
            <a-button shape="circle">
              <template #icon>
                <ArrowLeftOutlined />
              </template>
            </a-button>
          </router-link>
        </div>
        <div class="toolbars-right flex gap-2">
          <a-button type="primary" ghost @click="onSubmit" :loading="loading.isLoadingSave">{{ $t("Save") }}</a-button>
          <a-button type="primary" @click="onSubmit">{{
            $t("SaveAndAdd")
          }}</a-button>
          <a-button @click="resetForm">{{ $t("Cancel") }}</a-button>
        </div>
      </div>
      <div class="content">
        <a-form ref="formRef" :model="formState" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
          <a-form-item :label="$t('ComputerRoom.Name')" name="computerRoomId">
            <a-select v-model:value="formState.computerRoomId">
              <template v-for="cr in listComputerRoom">
                <a-select-option :value="cr.id">{{
                  cr.value
                }}</a-select-option>
              </template>
            </a-select>
          </a-form-item>
          <a-form-item :label="$t('Computer.Name')" name="name">
            <a-input v-model:value="formState.name" />
          </a-form-item>
          <a-form-item :label="$t('Computer.MacAddress')" name="macAddress">
            <a-input v-model:value="formState.macAddress" />
          </a-form-item>
          <a-form-item name="col" :label="$t('Computer.Col')">
            <a-input-number v-model:value="formState.col" />
          </a-form-item>
          <a-form-item name="row" :label="$t('Computer.Row')">
            <a-input-number v-model:value="formState.row" />
          </a-form-item>
          <a-form-item name="modifier" :label="$t('Computer.Condition')" class="collection-create-form_last-form-item">
            <a-select v-model:value="formState.listErrorId" mode="multiple" style="width: 100%"
              placeholder="Tình trạng máy" :options="optionListErrorId"></a-select>
          </a-form-item>
        </a-form>
      </div>
    </div>
  </a-spin>
</template>
<script setup>
import { reactive, ref, toRaw, onBeforeMount } from "vue";
import {
  useRoute,
  onBeforeRouteUpdate,
  onBeforeRouteLeave,
  useRouter,
} from "vue-router";
import { computerRoomService, computerService } from "@/api";
import { ResponseCode, FormMode } from "../../constants";
import { message } from "ant-design-vue";
import { ComputerKey } from "@/constants";
const route = useRoute();
const router = useRouter();
const formRef = ref();
const labelCol = {
  span: 5,
};
const wrapperCol = {
  span: 13,
};
let formState = reactive({
  computerRoomId: "",
  name: "",
  maxCapacity: 0,
  state: null,
});
const loading = reactive({
  isLoadingSave: false,
  isLoadingBeforeMount: false,
});
const isCallCheck = ref(false);
const optionListErrorId = reactive([
  {
    value: ComputerKey.ComputerError.Hardware,
    label: $t('Computer.ComputerError.Hardware')
  },
  {
    value: ComputerKey.ComputerError.Software,
    label: $t('Computer.ComputerError.Software')
  },
  {
    value: ComputerKey.ComputerError.Network,
    label: $t('Computer.ComputerError.Network')
  },
  {
    value: ComputerKey.ComputerError.OS,
    label: $t('Computer.ComputerError.OS')
  },
  {
    value: ComputerKey.ComputerError.Unknow,
    label: $t('Computer.ComputerError.Unknow')
  },
])
const validateName = async (_rule, value) => {
  if (value === "") {
    return Promise.reject($t("ComputerRoom.Validate.NameRequired"));
  } else if (isCallCheck.value == true) {
    isCallCheck.value = false;
    return Promise.reject($t("ComputerRoom.Validate.NameConflic"));
  } else {
    return Promise.resolve();
  }
};

const rules = {
  computerRoomId: [
    {
      required: true,
      message: $t("Validate.Required", [$t('ComputerRoom.Name')]),
      trigger: 'change',
    },
  ],
  name: [
    {
      required: true,
      validator: validateName,
      trigger: "change",
    },
  ],
  macAddress: [
    {
      required: true,
      message: $t("ComputerRoom.Validate.MaxCapacityRequired"),
      trigger: "change",
    },
  ],
  row: [
    {
      required: true,
      trigger: "change",
    },
  ],
  col: [
    {
      required: true,
      trigger: "change",
    },
  ],
};

const listComputerRoom = ref([]);

onBeforeMount(async () => {
  loading.isLoadingBeforeMount = true;
  try {
    await getListComputerRoom();
  } catch (error) {
    console.log(error);
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const onSubmit = async () => {
  let passValidate = false;
  try {
    loading.isLoadingSave = true;
    await formRef.value.validate();
    passValidate = true;
    try {
      let rs = await computerService.add(formState);
      if (rs?.success && rs?.data) {
        router.push({
          name: "ComputerView",
          params: { id: rs.data },
        });
      }
    } catch (error) {
      console.log(error);
      switch (error?.Code) {
        case ResponseCode.ComputerRoomNameConflic:
          isCallCheck.value = true;
          await formRef.value.validateFields("name");
          break;
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
const getListComputerRoom = async () => {
  try {
    let rs = await computerRoomService.getList({});
    if (rs && rs.data && rs.data.list) {
      listComputerRoom.value = rs.data.list.map(cr => {
        return {
          id: cr.id,
          value: cr.name
        }
      })
    }
  } catch (error) {
    console.log(error);
  }
}
</script>
