<template lang="">
  <a-layout class="app">
    <PageSidebar />
    <a-layout>
      <PageHeader />
      <a-layout-content style="height: calc(100vh - 48px);overflow:hidden">
        <slot></slot>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>
<script setup>
import PageHeader from "./PageHeader.vue";
import PageSidebar from "./PageSidebar.vue";
</script>
<style lang="scss" scoped>
.app {
  position: relative;
  height: 100vh;
  overflow: hidden;
}
</style>
