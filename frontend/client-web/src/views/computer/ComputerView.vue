<template>
  <div class="container-content">
    <div class="content">
      <a-tabs v-model:activeKey="activeKey">
        <template #leftExtra>
          <router-link :to="{ name: 'ComputerList' }">
            <a-button shape="circle" size="small" class="mr-2">
              <template #icon>
                <ArrowLeftOutlined />
              </template>
            </a-button>
          </router-link>
        </template>
        <a-tab-pane key="ComputerDetailView">
          <template #tab>
            <span>
              <apple-outlined />
              Thông tin chi tiết
            </span>
          </template>
          <ComputerDetailView />
        </a-tab-pane>
        <a-tab-pane key="ComputerHistoryView">
          <template #tab>
            <span>
              <android-outlined />
              Lịch sử truy cập
            </span>
          </template>
          <ComputerHistoryView />
        </a-tab-pane>
        <a-tab-pane key="ComputerNoteView">
          <template #tab>
            <span>
              <apple-outlined />
              Ghi chú
            </span>
          </template>
          <ComputerNoteView />
        </a-tab-pane>
        <a-tab-pane key="ComputerSoftwareView">
          <template #tab>
            <span>
              <android-outlined />
              Phần mềm cài đặt
            </span>
          </template>
          Phan mem cai dat
        </a-tab-pane>
      </a-tabs>
    </div>
  </div>
</template>
<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import ComputerDetailView from "./ComputerDetailView.vue";
import ComputerHistoryView from "./ComputerHistoryView.vue";
import ComputerNoteView from "./ComputerNoteView.vue";

const router = useRouter();
const activeKey = ref("ComputerDetailView");
</script>
<style lang=""></style>
