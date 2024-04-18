<template>
  <div class="container">
    <div class="table-operations flex justify-between">
      <div class="operations-left">
        <a-switch v-model:checked="typeView" checked-children="Sơ đồ" un-checked-children="Bảng" />
      </div>
      <div class="operations-right"></div>
    </div>
    <div class="content">
      <template v-if="typeView">
        <ComputerRoomComputerGrid />
      </template>
      <template v-else>
        <ComputerRoomComputerTable />
      </template>
    </div>
  </div>
</template>
<script setup>
  import {  ref } from "vue";
  import { useRoute, useRouter } from "vue-router";
  import _ from "lodash";
  import ComputerRoomComputerGrid from "./ComputerRoomComputerGrid.vue";
  import ComputerRoomComputerTable from "./ComputerRoomComputerTable.vue";
  const router = useRouter();
  const route = useRoute();
  const typeView = ref(true);
 
</script>
<style lang="scss" scoped>
  .container {
    .table-operations {
      margin-bottom: 16px;
    }

    .table-operations > button {
      margin-right: 8px;
    }
  }
</style>
