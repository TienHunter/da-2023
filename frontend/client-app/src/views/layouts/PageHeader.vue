<template lang="">
  <a-layout-header
    class="h-12 px-4"
    style="
      background: #fff;
      height: 3rem;
      line-height: 3rem;
      padding-inline: 1.5rem;
    "
  >
    <div class="h-full flex items-center gap-3 justify-end">
      <BellOutlined class="font-20 font-bold pointer" />
      <QuestionCircleOutlined class="font-20 font-bold pointer" />
      <a-dropdown trigger="['click']" style="line-height: 3rem">
        <template #overlay>
          <a-menu>
            <a-menu-item key="1">
              <UserOutlined />
              {{ $t("auth.Profile") }}
            </a-menu-item>
            <a-menu-item key="2" @click="logout">
              <LogoutOutlined />
              {{ $t("auth.Logout") }}
            </a-menu-item>
          </a-menu>
        </template>
        <a-avatar
          size="medium"
          :style="{ backgroundColor: 'red', verticalAlign: 'middle' }"
          :gap="100"
          class="pointer"
        >
          <template #icon><UserOutlined /></template>
        </a-avatar>
      </a-dropdown>
    </div>
  </a-layout-header>
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

  const logout = async () => {
    try {
      await userService.logout();
      window.location.href = "/login";
    } catch (error) {
      message.error();
    }
  };
</script>
<style lang=""></style>
