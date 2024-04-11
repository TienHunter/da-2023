<template>
  <a-spin tip="Loading..." :spinning="loading.isLoadingBeforeMount">
    <div class="container">
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
          <a-button
            type="primary"
            ghost
            @click="onSubmit"
            :loading="loading.isLoadingSave"
            >{{ $t("Save") }}</a-button
          >
          <a-button type="primary" @click="onSubmit">{{
            $t("SaveAndAdd")
          }}</a-button>
          <a-button @click="resetForm">{{ $t("Cancel") }}</a-button>
        </div>
      </div>
      <div class="content">
        <a-form
          ref="formRef"
          :model="formState"
          :rules="rules"
          :label-col="labelCol"
          :wrapper-col="wrapperCol"
        >
          <a-form-item :label="$t('ComputerRoom.Name')" name="name">
            <a-input v-model:value="formState.name" />
          </a-form-item>
          <a-form-item
            :label="$t('ComputerRoom.MaxCapacity')"
            name="maxCapacity"
          >
            <a-input-number
              v-model:value="formState.maxCapacity"
              :min="1"
              :max="1000"
            />
          </a-form-item>
          <a-form-item :label="$t('ComputerRoom.State')" name="state">
            <a-select
              v-model:value="formState.state"
              :placeholder="$t('ComputerRoom.StateHint')"
            >
              <a-select-option :value="0">Hỏng</a-select-option>
              <a-select-option :value="1">Tốt</a-select-option>
              <a-select-option :value="2">Bảo trì</a-select-option>
            </a-select>
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
  import { computerRoomService } from "@/api";
  import { ResponseCode, FormMode } from "../../constants";
  import { message } from "ant-design-vue";
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
    name: "",
    maxCapacity: 0,
    state: null,
  });
  const loading = reactive({
    isLoadingSave: false,
    isLoadingBeforeMount: false,
  });
  const isCallCheck = ref(false);
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
    name: [
      {
        required: true,
        validator: validateName,
        trigger: "change",
      },
    ],
    maxCapacity: [
      {
        required: true,
        message: $t("ComputerRoom.Validate.MaxCapacityRequired"),
        trigger: "change",
      },
    ],
    state: [
      {
        required: true,
        message: $t("ComputerRoom.Validate.StateRequired"),
        trigger: "change",
      },
    ],
  };

  onBeforeMount(async () => {
    if (route.meta.formMode === FormMode.Update) {
      loading.isLoadingBeforeMount = true;
      try {
        let computer = await computerRoomService.getById(route.params.id);
        if (computer?.success && computer?.data) {
          formState = reactive(computer.data);
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
        router.push({ name: "ComputerRoomList" });
      } finally {
        loading.isLoadingBeforeMount = false;
      }
    }
  });

  const onSubmit = async () => {
    let passValidate = false;
    try {
      loading.isLoadingSave = true;
      await formRef.value.validate();
      passValidate = true;
      try {
        let computer =
          route.meta.formMode === FormMode.Update
            ? await computerRoomService.update(formState, route.params.id)
            : await computerRoomService.add(formState);
        if (computer?.success && computer?.data) {
          router.push({
            name: "ComputerRoomView",
            params: { id: computer.id },
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
</script>
