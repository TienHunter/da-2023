<template>
  <div class="container-login">
    <div class="wrap-login">
      <a-form
        ref="formRef"
        name="custom-validation"
        :model="registerInfo"
        :rules="rules"
        autocomplete="off"
        size="large"
        class="w-full"
        layout="vertical"
        @finish="onFinish"
        @validate="onValidate"
      >
        <a-form-item style="margin-bottom: -12px" :wrapper-col="{ span: 24 }">
          <h2 class="login-title m-0">{{ $t("auth.Register") }}</h2>
        </a-form-item>
        <a-form-item name="fullname" :label="$t('auth.Fullname')">
          <a-input
            v-model:value="registerInfo.fullname"
            :placeholder="$t('auth.Fullname')"
            autocomplete="off"
          />
        </a-form-item>
        <a-form-item name="username" :label="$t('auth.UsernameHint')">
          <a-input
            v-model:value="registerInfo.username"
            :placeholder="$t('auth.UsernameHint')"
            autocomplete="off"
          />
        </a-form-item>
        <a-form-item name="email" :label="$t('auth.EmailHint')">
          <a-input
            v-model:value="registerInfo.email"
            :placeholder="$t('auth.EmailHint')"
            autocomplete="off"
          />
        </a-form-item>
        <a-form-item name="password" :label="$t('auth.PasswordHint')">
          <a-input-password
            v-model:value="registerInfo.password"
            autocomplete="off"
          />
        </a-form-item>
        <a-form-item
          name="confirmPassword"
          :label="$t('auth.ConfirmPasswordHint')"
        >
          <a-input-password
            v-model:value="registerInfo.confirmPassword"
            autocomplete="off"
          />
        </a-form-item>
        <a-form-item>
          <a-button
            type="primary"
            class="w-full"
            html-type="submit"
            :loading="isLoading"
            >{{ $t("auth.Register") }}</a-button
          >
        </a-form-item>

        <!-- Đường dẫn "login" -->
        <a-form-item>
          <span class="w-full flex justify-center gap-2"
            >{{ $t("auth.YouHaveAccount") }}
            <a href="/login">{{ $t("auth.Login") }}</a></span
          >
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
  const registerInfo = reactive({
    fullname: "",
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const isCallCheck = reactive({
    username: false,
    email: false,
  });
  const isLoading = ref(false);
  const formRef = ref();
  const validateUsername = async (_rule, value) => {
    if (value === "") {
      return Promise.reject($t("auth.UsernameRegsiterRequired"));
    } else if (isCallCheck.username == true) {
      isCallCheck.username = false;
      try {
        let userInfo = await userService.register(registerInfo);
        message.success($t("auth.RegisterSuccess"));
        return Promise.resolve();
      } catch (error) {
        switch (error?.Code) {
          case ResponseCode.UsernameConflict:
            return Promise.reject($t("auth.UsernameConflict"));
            break;
          case ResponseCode.EmailConflict:
            isCallCheck.email = true;
            formRef.value.validateFields("email");
            return Promise.resolve();
            break;
          default:
            message.error("create job failure");
            return Promise.resolve($t("UnKnowError"));
            break;
        }
      }
    } else {
      return Promise.resolve();
    }
  };
  const validateEmail = async (_rule, value) => {
    if (value === "") {
      return Promise.reject($t("auth.PasswordRequired"));
    } else if (isCallCheck.email == true) {
      isCallCheck.email = false;
      return Promise.reject($t("auth.EmailConflict"));
    } else {
      return Promise.resolve();
    }
  };
  const validatePassword = async (_rule, value) => {
    if (value === "") {
      return Promise.reject($t("auth.PasswordRequired"));
    } else {
      if (registerInfo.confirmPassword) {
        formRef.value.validateFields("confirmPassword");
      }
      return Promise.resolve();
    }
  };
  const validateConfirmPassword = async (_rule, value) => {
    if (value === "") {
      return Promise.reject($t("auth.ConfirmPasswordRequired"));
    } else if (value !== registerInfo.password) {
      return Promise.reject($t("auth.WrongConfirmPassword"));
    } else {
      return Promise.resolve();
    }
  };

  const rules = {
    fullname: [
      {
        required: true,
        message: $t("auth.FullnameRequired"),
      },
    ],
    username: [
      {
        required: true,
        validator: validateUsername,
      },
    ],
    email: [
      {
        required: true,
        validator: validateEmail,
      },
      {
        type: "email",
        message: $t("auth.EmailInvalid"),
      },
    ],
    password: [
      {
        required: true,
        validator: validatePassword,
      },
    ],
    confirmPassword: [
      {
        required: true,
        validator: validateConfirmPassword,
      },
    ],
  };
  const onFinish = async (value) => {
    try {
      isCallCheck.username = true;
      // await formRef.value.validate();
      await formRef.value.validateFields("username");
      router.push({ name: "login" });
    } catch (error) {
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
