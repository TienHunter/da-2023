<template lang="">
  <a-layout-sider
    v-model:collapsed="collapsed"
    id="components-layout-demo-side"
    collapsible
  >
    <div class="logo" />
    <a-menu v-model:selectedKeys="selectedKeys" theme="dark" mode="inline">
      <a-menu-item key="Dashboard">
        <router-link :to="{ name: 'Dashboard' }">
          <pie-chart-outlined />
          <span>Dashboard</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="ComputerRoom">
        <router-link :to="{ name: 'ComputerRoomList' }">
          <desktop-outlined />
          <span>ComputerRoom</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="Computer">
        <router-link :to="{ name: 'ComputerList' }">
          <desktop-outlined />
          <span>Computer</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="User">
        <router-link :to="{ name: 'UserList' }">
          <user-outlined />
          <span>User manage</span>
        </router-link>
      </a-menu-item>
      <a-sub-menu key="sub2">
        <template #title>
          <span>
            <team-outlined />
            <span>Team</span>
          </span>
        </template>
        <a-menu-item key="6">Team 1</a-menu-item>
        <a-menu-item key="8">Team 2</a-menu-item>
      </a-sub-menu>
      <a-menu-item key="History">
        <file-outlined />
        <span>File</span>
      </a-menu-item>
    </a-menu>
  </a-layout-sider>
</template>
<script setup>
  import { ref, onBeforeMount, computed } from "vue";
  import { useRouter, useRoute } from "vue-router";
  const route = useRoute();
  const router = useRouter();
  const collapsed = ref(false);
  const selectedKeys = ref([]);
  onBeforeMount(() => {
    selectedKeys.value = [route.matched[0]?.name];
  });
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
