<template>
  <a-layout-sider v-model:collapsed="collapsed" id="components-layout-demo-side" collapsible>
    <div class="logo" />
    <a-menu v-model:selectedKeys="selectedKeys" theme="dark" mode="inline">
      <a-menu-item key="Dashboard">
        <router-link :to="{ name: 'Dashboard' }">
          <pie-chart-outlined />
          <span>{{ $t("Sidebar.Dashboard") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="ComputerRoom">
        <router-link :to="{ name: 'ComputerRoomList' }">
          <HomeOutlined />
          <span>{{ $t("Sidebar.ComputerRoomManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="Computer">
        <router-link :to="{ name: 'ComputerList' }">
          <desktop-outlined />
          <span>{{ $t("Sidebar.ComputerManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="Software">
        <router-link :to="{ name: 'SoftwareList' }">
          <ShoppingOutlined />
          <span>{{ $t("Sidebar.SoftwareManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="File">
        <router-link :to="{ name: 'FileList' }">
          <file-outlined />
          <span>{{ $t("Sidebar.FileManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="MonitorSession">
        <router-link :to="{ name: 'MonitorSessionList' }">
          <VideoCameraOutlined />
          <span>{{ $t("Sidebar.MonitorSessionManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item v-if="checkShowByRole([UserRole.Admin])" key="ConfigOption">
        <router-link :to="{ name: 'ConfigOptionList' }">
          <SettingOutlined />
          <span>{{ $t("Sidebar.ConfigOptionManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item v-if="checkShowByRole([UserRole.Admin])" key="Agent">
        <router-link :to="{ name: 'Agent' }">
          <WindowsOutlined />
          <span>{{ $t("Sidebar.AgentManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="Student">
        <router-link :to="{ name: 'Student' }">
          <UsergroupAddOutlined />
          <span>{{ $t("Sidebar.StudentManager") }}</span>
        </router-link>
      </a-menu-item>
      <a-menu-item v-if="checkShowByRole([UserRole.Admin])" key="User">
        <router-link :to="{ name: 'UserList' }">
          <user-outlined />
          <span>{{ $t("Sidebar.UserManager") }}</span>
        </router-link>
      </a-menu-item>

    </a-menu>
  </a-layout-sider>
</template>
<script setup>
import { LocalStorageKey, UserRole } from "@/constants";
import { localStore } from "@/utils";
import { ref, onBeforeMount, computed, watchEffect } from "vue";
import { useRouter, useRoute } from "vue-router";
const route = useRoute();
const router = useRouter();
const collapsed = ref(false);
const selectedKeys = ref([]);
const userInfo = localStore.getItem(LocalStorageKey.userInfor);
watchEffect(() => {
  selectedKeys.value = [route.matched[0]?.name];
});

const checkShowByRole = (roles) => {
  let rs = false;
  if (!roles?.length) {
    rs = true;
  }
  else if (userInfo && roles.includes(userInfo?.roleID)) {
    rs = true;
  }
  return rs;
}
</script>
<style scoped>
#components-layout-demo-side .logo {
  height: 32px;
  margin: 16px;
  background: rgba(255, 255, 255, 0.3);
}

.site-layout .site-layout-background {
  background: #fff;
}

[data-theme="dark"] .site-layout .site-layout-background {
  background: #141414;
}
</style>
