<template>
  <a-layout-header class="h-12 px-4" style="
      background: #fff;
      height: 3rem;
      line-height: 3rem;
      padding-inline: 1.5rem;
    ">
    <div class="h-full flex items-center gap-3 justify-end">
      <BellOutlined class="font-20 font-bold pointer" />
      <QuestionCircleOutlined class="font-20 font-bold pointer" />
      <a-dropdown trigger="['click']" style="line-height: 3rem">
        <template #overlay>
          <div class="bg-white flex flex-col text-center p-4 rounded-lg  border-solid border-2 border-gray-200 ">
            <!-- thông tin tóm tắt -->
            <div>{{ user?.email }}</div>
            <div class="font-bold">{{ user?.fullname }}</div>
            <a-divider style="margin: 12px 0px;" />
            <a-button type="text" class="text-left">
              <template #icon>
                <UserOutlined />
              </template>
              {{ $t("auth.Profile") }}
            </a-button>
            <a-button type="text" class="text-left" @click="toggleShowModalChangePassowrd(true)">
              <template #icon>
                <KeyOutlined />
              </template>
              {{ $t("auth.ChangePassword") }}
            </a-button>
            <a-button type="text" class="text-left" @click="logout">
              <template #icon>
                <LogoutOutlined />
              </template>
              {{ $t("auth.Logout") }}
            </a-button>
            <!-- <a-menu>
              <a-menu-item key="1">
                <UserOutlined />
                {{ $t("auth.Profile") }}
              </a-menu-item>
              <a-menu-item key="2" @click="logout">
                <LogoutOutlined />
                {{ $t("auth.Logout") }}
              </a-menu-item>
            </a-menu> -->
          </div>

        </template>
        <a-avatar size="medium" :style="{ backgroundColor: 'red', verticalAlign: 'middle' }" :gap="100" class="pointer">
          <template #icon>
            <UserOutlined />
          </template>
        </a-avatar>
      </a-dropdown>
    </div>
  </a-layout-header>

  <ChangePasswordMoal v-if="isShowModalChangePassword" @toggleShowModal="toggleShowModalChangePassowrd"
    @afterSave="afterChangePassword" />
</template>
<script setup>
import {
  DownOutlined,
  UserOutlined,
  FormOutlined,
  RadiusUprightOutlined,
  UsergroupAddOutlined,
  BellOutlined,
  QuestionCircleOutlined,
} from "@ant-design/icons-vue";
import { userService } from "@/api";
import { onBeforeMount, ref } from "vue";
import { localStore } from "@/utils";
import { LocalStorageKey } from "@/constants";
import ChangePasswordMoal from "../user/ChangePasswordMoal.vue";
import { message } from "ant-design-vue";
// ========== start state ========== 
const user = ref({});
const isShowModalChangePassword = ref(false);
// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(() => {
  user.value = localStore.getItem(LocalStorageKey.userInfor);
  console.log("render");
})
// ========== end lifecycle ==========

const logout = () => {
  try {
    localStorage.clear();
    window.location.href = "/login";
  } catch (error) {
    message.error();
  }
};

const toggleShowModalChangePassowrd = (isShow) => {
  isShowModalChangePassword.value = isShow
}

const afterChangePassword = () => {
  message.success($t("auth.ChangePasswordSuccess"));
  isShowModalChangePassword.value = false;
}
</script>
<style lang="scss"></style>
