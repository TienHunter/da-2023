<template>
  <a-list item-layout="horizontal" :data-source="datas">
    <template #renderItem="{ item }">
      <a-list-item>
        <a-list-item-meta :description="item.message">
          <template #title>
            <span>{{ item.logTime }}</span>
          </template>
          <!-- <template #avatar>
            <a-avatar src="https://joeschmoe.io/api/v1/random" />
          </template> -->
        </a-list-item-meta>
      </a-list-item>
    </template>
  </a-list>
</template>
<script setup>
import { computerHistoryService } from "@/api";
import { FormatDateKey } from "@/constants";
import { message } from "ant-design-vue";
import _ from "lodash";
import moment from "moment";
import { onBeforeMount, ref } from "vue";
import { useRoute } from "vue-router";
// ========== start property ==========
const route = useRoute();
const datas = ref([]);
const paging = {
  pageNumber: 1,
  pageSize: 10,
};
// ========== end property ==========

// ========== start lifecycle ==========
onBeforeMount(async () => {
  // lắng nghe socket

  // lấy danh sách
  try {
    let rs = await computerHistoryService.getListByComputerId(
      route.params.id,
      paging
    );
    if (rs?.success && rs?.data) {
      datas.value.push(
        ..._.cloneDeep(
          rs.data.list.map((item) => {
            item.logTime = item.logTime
              ? moment(item.logTime).format(FormatDateKey.Default)
              : "";
            return item;
          })
        )
      );
    }
  } catch (error) {
    console.log(error);
    message.error($t("UnknownError"));
  }
});
// ========== end lifecycle ==========

// ========== start methods ==========
// Your code here
// ========== end methods ==========
</script>
