<template lang="">
  <div class="contailer">
    <div class="toolbars flex justify-between p-4 rounded">
      <div class="toolbars-left">
        <router-link :to="{ name: 'ComputerRoomList' }">
          <a-button shape="circle">
            <template #icon>
              <ArrowLeftOutlined />
            </template>
          </a-button>
        </router-link>
      </div>
      <div class="toolbars-right flex gap-2">
        <a-button
          type="primary"
          ghost
          @click="onSubmit"
          :loading="loading.isLoadingSave"
          >{{ $t("Save") }}</a-button
        >
        <a-button type="primary" @click="onSubmit">{{
          $t("SaveAndAdd")
        }}</a-button>
        <a-button @click="resetForm">{{ $t("Cancel") }}</a-button>
      </div>
    </div>
    <div class="content"></div>
  </div>
</template>
<script setup>
  import { ref, reactive, onBeforeMount } from "vue";
  import { useRoute } from "vue-router";
  const fields = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Capacity",
      dataIndex: "capacity",
      key: "capacity",
    },
    {
      title: "Pending",
      dataIndex: "pending",
      key: "pending",
    },
  ];
  let datas = reactive({});
  const loading = reactive({
    isLoadingBeforeMount: false,
  });
  onBeforeMount(async () => {
    try {
      let computerRoom = await computerRoomService;
    } catch (error) {
    } finally {
    }
  });
</script>
<style lang=""></style>
