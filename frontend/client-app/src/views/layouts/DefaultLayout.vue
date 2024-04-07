<template lang="">
  <a-layout class="app">
    <PageSidebar />
    <a-layout>
      <PageHeader />
      <a-layout-content style="margin: 16px">
        <slot></slot>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>
<script setup>
  import PageHeader from "./PageHeader.vue";
  import PageSidebar from "./PageSidebar.vue";
</script>
<style scoped>
  .app {
    position: relative;
    height: 100vh;
    overflow: hidden;
  }
</style>
