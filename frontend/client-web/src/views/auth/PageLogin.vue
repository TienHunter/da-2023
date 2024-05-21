<template>
  <div class="container-login">
    <div class="wrap-login">
      <a-form ref="formLoginRef" name="custom-validation" :model="loginInfo" :rules="rules" autocomplete="off"
        size="large" class="w-full" layout="vertical" @finish="onFinish">
        <a-form-item :wrapper-col="{ span: 24 }">
          <h2 class="login-title">{{ $t("auth.Login") }}</h2>
        </a-form-item>
        <a-form-item name="username" :label="$t('auth.UsernameHint')">
          <a-input v-model:value="loginInfo.username" :placeholder="$t('auth.UsernameHint')" autocomplete="off" />
        </a-form-item>

        <a-form-item name="password" :label="$t('auth.PasswordHint')">
          <a-input-password v-model:value="loginInfo.password" :placeholder="$t('auth.PasswordHint')"
            autocomplete="off" />
        </a-form-item>

        <!-- Đường dẫn "Quên mật khẩu" -->
        <a-form-item style="margin-bottom: -12px">
          <p class="flex justify-center m-0">
            <a href="/forgot-password">{{ $t("auth.Forgotpassword") }}</a>
          </p>
        </a-form-item>

        <a-form-item>
          <a-button type="primary" class="w-full" html-type="submit" :loading="isLoading">{{ $t("auth.Login")
            }}</a-button>
        </a-form-item>

        <!-- Đường dẫn "Register" -->
        <a-form-item>
          <span class="w-full flex justify-center gap-2">{{ $t("auth.YouDontHaveAccount") }}
            <a href="/register">{{ $t("auth.Register") }}</a></span>
        </a-form-item>
      </a-form>
    </div>
  </div>
</template>
<script setup>
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { userService } from "@/api";
import { useI18n } from "vue-i18n";
import { localStore } from "../../utils";
import { LocalStorageKey, ResponseCode } from "@/constants";
import { message } from "ant-design-vue";
const router = useRouter();
const loginInfo = reactive({
  username: "",
  password: "",
});
const isCallCheck = ref(false);
const isLoading = ref(false);
const formLoginRef = ref();
const validatePassword = async (_rule, value) => {
  if (value === "") {
    return Promise.reject($t("auth.PasswordRequired"));
  } else if (isCallCheck.value == true) {
    isCallCheck.value = false;
    try {
      let userInfo = await userService.login(loginInfo);
      if (userInfo.success && userInfo?.data) {
        localStore.setItem(
          LocalStorageKey.accessToken,
          userInfo.data.accessToken
        );
        localStore.setItem(LocalStorageKey.userInfor, {
          id: userInfo.data.id,
          email: userInfo.data.email,
          fullname: userInfo.data.fullname,
          username: userInfo.data.username,
          roleID: userInfo.data.roleID,
        });
      }
      message.success($t("auth.LoginSuccess"));
      return Promise.resolve();
    } catch (error) {
      switch (error?.Code) {
        case ResponseCode.WrongLogin:
          return Promise.reject($t("auth.WrongLogin"));
          break;
        case ResponseCode.UserPending:
          message.warning($t("auth.UserPending"));
          break;
        case ResponseCode.UserRevoked:
          message.warning($t("auth.UserRevoked"));
          break;
        default:
          message.error($t("UnknownError"));
          return Promise.resolve();
          break;
      }
    }
  } else {
    return Promise.resolve();
  }
};

const rules = {
  username: [
    {
      required: true,
      message: $t("auth.UsernameRequired"),
    },
  ],
  password: [
    {
      required: true,
      validator: validatePassword,
    },
  ],
};
const onFinish = async (value) => {
  try {
    isLoading.value = true;
    isCallCheck.value = true;
    await formLoginRef.value.validateFields("password");
    router.push({ name: "Dashboard" });
  } catch (error) {
    console.log(error);
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
.container-login {
  width: 100%;
  min-height: 100vh;
  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
  padding: 15px;
  background-image: url(../../assets//images//login.jpg);
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  position: relative;
  z-index: 1;
}

.wrap-login {
  background-color: #ffffffcc;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 12px;
  border-radius: 4px;
  width: 360px;
}

.login-title {
  display: flex;
  justify-content: center;
  font-size: 24px;
}

.w-full {
  width: 100%;
}
</style>
