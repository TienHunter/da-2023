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
          <a-button type="primary" ghost :loading="loading.isLoadingSave" v-has-permission="`${UserRole.Admin}`"
            v-passPermissionClick="() => onSubmit(1)">{{ $t("Save") }}</a-button>
          <a-button type="primary" v-has-permission="`${UserRole.Admin}`" v-passPermissionClick="() => onSubmit(2)">{{
            $t("SaveAndAdd")
          }}</a-button>
          <a-button @click="resetForm">{{ $t("Cancel") }}</a-button>
        </div>
      </div>
      <div class="content">
        <a-form ref="formRef" :model="formState" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
          <a-form-item :label="$t('ComputerRoom.Name')" name="computerRoomId">
            <a-select v-model:value="formState.computerRoomId" show-search :field-names="{ label: 'name', value: 'id' }"
              :filter-option="filterOption" :options="listComputerRoom">
              <!-- <template v-for="cr in listComputerRoom">
                <a-select-option :value="cr.id">{{
                  cr.value
                }}</a-select-option>
              </template> -->
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
import { reactive, ref, toRaw, onBeforeMount, watch } from "vue";
import {
  useRoute,
  onBeforeRouteUpdate,
  onBeforeRouteLeave,
  useRouter,
} from "vue-router";
import { computerRoomService, computerService } from "@/api";
import { ResponseCode, FormMode } from "../../constants";
import { message } from "ant-design-vue";
import { ComputerKey, UserRole } from "@/constants";
const route = useRoute();
const router = useRouter();
const formRef = ref();
const labelCol = {
  span: 5,
};
const wrapperCol = {
  span: 13,
};
let formState = ref({
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

const listComputerRoom = ref([]);
const computerRoomSelect = ref(null);
const errorCode = ref(null);
const filterOption = (input, option) => {
  return option.name.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const validateName = async (_rule, value) => {
  if (value === "") {
    return Promise.reject($t("Validate.Required", [$t("Computer.Name")]));
  } else if (errorCode.value == ResponseCode.ConflicNameComputer) {
    errorCode.value = 0;
    return Promise.reject($t("Computer.Validate.ConflicNameComputer"));
  }
  else {
    return Promise.resolve();
  }
};
const validateMacAddress = async (_rule, value) => {
  if (value === "") {
    return Promise.reject($t("Validate.Required", [$t("Computer.MacAddress")]));
  } else if (errorCode.value == ResponseCode.ConflicMacAddress) {
    errorCode.value = 0;
    return Promise.reject($t("Computer.Validate.ConflicMacAddressComputer"));
  }
  else {
    return Promise.resolve();
  }
};
const checkRow = async (_rule, value) => {
  if (!value) {
    return Promise.reject(
      $t("Validate.Required", [$t("Computer.Row")])
    );
  }
  if (!Number.isInteger(value)) {
    return Promise.reject(
      $t("Validate.IntegerType", [$t("Computer.Row")])
    );
  } else {
    if (computerRoomSelect.value && (value < 1 || value > computerRoomSelect.value.row)) {
      return Promise.reject(
        $t("Validate.Range", [$t("Computer.Row"), 1, computerRoomSelect.value.row])
      );
    } else {
      return Promise.resolve();
    }
  }
};
const checkCol = async (_rule, value) => {
  if (!value) {
    return Promise.reject(
      $t("Validate.Required", [$t("Computer.Col")])
    );
  }
  else if (!Number.isInteger(value)) {
    return Promise.reject(
      $t("Validate.IntegerType", [$t("Computer.Col")])
    );
  }
  else if (errorCode.value == ResponseCode.ConflicRowColComputer) {
    errorCode.value = 0;
    return Promise.reject(
      $t("Computer.Validate.ConflicRowColComputer")
    );
  } else if (errorCode.value == ResponseCode.InValidRowColComputer) {
    errorCode.value = 0;
    return Promise.reject(
      $t("Computer.Validate.InValidRowColComputer")
    );
  }
  else {
    if (computerRoomSelect.value && (value < 1 || value > computerRoomSelect.value.col)) {
      return Promise.reject(
        $t("Validate.Range", [$t("Computer.Col"), 1, computerRoomSelect.value.col])
      );
    } else {
      return Promise.resolve();
    }
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
      validator: validateMacAddress,
      trigger: "change",
    },
  ],
  row: [
    {
      required: true,
      validator: checkRow,
      trigger: "change",
    },
  ],
  col: [
    {
      required: true,
      validator: checkCol,
      trigger: "change",
    },
  ],
};


onBeforeMount(async () => {
  loading.isLoadingBeforeMount = true;
  try {
    await getListComputerRoom();
    if (route.meta.formMode === FormMode.Update) {
      try {
        let rs = await computerService.getById(route.params.id);
        if (rs?.success && rs?.data) {
          formState.value = rs.data;
          computerRoomSelect.value = _.cloneDeep(rs.data.computerRoom);
        }
      } catch (error) {
        switch (error?.Code) {
          case ResponseCode.NotFoundComputer:
            message.error($t("Computer.Validate.NotFound"));
            break;
          default:
            message.error($t("UnknownError"));
            break;
        }
        router.push({ name: "ComputerList" });
      }
    }
  } catch (error) {
    console.log(error);
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

/**
 * gán lại computer room select
 */
watch(() => formState.value.computerRoomId, () => {
  computerRoomSelect.value = listComputerRoom.value.find(cr => cr.id = formState.value.computerRoomId);
})

const onSubmit = async (key) => {
  let passValidate = false;
  try {
    loading.isLoadingSave = true;
    await formRef.value.validate();
    passValidate = true;
    try {
      let rs = route.meta.formMode === FormMode.Update ? await computerService.update(formState.value, route.params.id) : await computerService.add(formState.value);
      if (rs?.success && rs?.data) {
        if (key == 2) {
          resetFormState();
        } else {
          router.push({
            name: "ComputerView",
            params: { id: rs.data },
          });
        }

      }
    } catch (error) {
      console.log(error);
      if (error?.Code) {
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
</script>
