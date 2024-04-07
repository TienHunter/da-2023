<script setup>
  import { useRoute } from "vue-router";
  import HelloWorld from "./components/HelloWorld.vue";
  import { computed } from "vue";
  const route = useRoute();
  const layout = computed(() => (route.meta?.layout || "default") + "-layout");
</script>

<template>
  <component :is="layout">
    <router-view />
  </component>
</template>

<style scoped>
  .logo {
    height: 6em;
    padding: 1.5em;
    will-change: filter;
    transition: filter 300ms;
  }
  .logo:hover {
    filter: drop-shadow(0 0 2em #646cffaa);
  }
  .logo.vue:hover {
    filter: drop-shadow(0 0 2em #42b883aa);
  }
</style>
