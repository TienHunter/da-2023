<template>
  <div class="container-content">
    <a-row :gutter="[16, 16]">
      <a-col :span="12">
        <ChartComputerOnline />
      </a-col>
      <a-col :span="12"></a-col>
      <a-col :span="24">
        <ChartComputerCondition />
      </a-col>
    </a-row>
  </div>
</template>
<script setup>
import ChartComputerOnline from "./ChartComputerOnline.vue";
import ChartComputerCondition from "./ChartComputerCondition.vue";
</script>
<style lang="scss" scoped>
.container-content {
  padding: 16px;
}
</style>
