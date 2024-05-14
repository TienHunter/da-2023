<template>
  <a-spin tip="Loading..." :spinning="loading.isLoadingBeforeMount">
    <div class="container-content">
      <div class="toolbars flex justify-between p-4 rounded">
        <div class="toolbars-left">
          <router-link :to="{ name: 'UserList' }">
            <a-button shape="circle">
              <template #icon>
                <ArrowLeftOutlined />
              </template>
            </a-button>
          </router-link>
        </div>
        <div class="toolbars-right flex gap-2">
          <a-button type="primary" ghost @click="onSubmit" :loading="loading.isLoadingSave">{{ $t("Save") }}</a-button>
          <a-button>{{ $t("Cancel") }}</a-button>
        </div>
      </div>
      <div class="content">
        <a-form ref="formRef" :model="formState" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
          <a-form-item :label="$t('User.Username')" name="username">
            <a-input v-model:value="formState.username" :disabled="true" />
          </a-form-item>
          <a-form-item :label="$t('User.Email')" name="username">
            <a-input v-model:value="formState.email" :disabled="true" />
          </a-form-item>
          <a-form-item :label="$t('User.Fullname')" name="fullname">
            <a-input v-model:value="formState.fullname" :disabled="true" />
          </a-form-item>
          <a-form-item :label="$t('User.Role')" name="roleID">
            <a-select v-model:value="formState.roleID">
              <a-select-option :value="1">{{
                $t("User.RoleAdmin")
              }}</a-select-option>
              <a-select-option :value="2">{{
                $t("User.RoleTeacher")
              }}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="$t('User.State')" name="state">
            <a-select v-model:value="formState.state">
              <a-select-option :value="0">{{
                $t("User.UserPending")
              }}</a-select-option>
              <a-select-option :value="1">{{
                $t("User.UserActive")
              }}</a-select-option>
              <a-select-option :value="2">{{
                $t("User.UserRevoked")
              }}</a-select-option>
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
import { computerRoomService, userService } from "@/api";
import { ResponseCode, FormMode } from "../../constants";
import { message } from "ant-design-vue";
import _ from "lodash";
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
  username: "",
  email: "",
  fullname: "",
  roleID: null,
  state: null,
});
const loading = reactive({
  isLoadingSave: false,
  isLoadingBeforeMount: false,
});
const rules = {};

onBeforeMount(async () => {
  loading.isLoadingBeforeMount = true;
  try {
    let user = await userService.getById(route.params.id);
    if (user?.success && user?.data) {
      formState = reactive(_.cloneDeep(user.data));
    }
  } catch (error) {
    switch (error?.Code) {
      case ResponseCode.NotFoundUser:
        message.error($t("User.NotFoundUser"));
        break;
      default:
        message.error($t("UnKnowError"));
        break;
    }
    router.push({ name: "UserList" });
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const onSubmit = async () => {
  try {
    loading.isLoadingSave = true;
    try {
      let user = await userService.updateByAdmin(formState, route.params.id);
      if (user?.success && user?.data) {
        router.push({
          name: "UserView",
          params: { id: user.id },
        });
      }
    } catch (error) {
      console.log(error);
      switch (error?.Code) {
        case ResponseCode.ComputerRoomNameConflic:
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
