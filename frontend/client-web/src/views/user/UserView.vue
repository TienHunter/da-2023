<template>
  <div class="container-content">
    <div class="toolbars flex justify-between py-4 rounded">
      <div class="toolbars-left">
        <router-link :to="{ name: 'UserList' }">
          <a-button shape="circle">
            <template #icon>
              <ArrowLeftOutlined />
            </template>
          </a-button>
        </router-link>
      </div>
      <div class="toolbars-right flex gap-2">

        <a-button type="primary" ghost v-has-permission="`${UserRole.Admin}`"
          v-passPermissionClick="() => navigateEdit()">{{
            $t("Edit") }}</a-button>
      </div>
    </div>
    <div class="content">
      <div class="master">
        <a-row :gutter="[16, 24]">
          <template v-for="field in fields" :key="field.dataIndex">
            <a-col class="gutter-row" :span="6">
              <label class="font-bold">{{ field.title }}</label>
            </a-col>

            <a-col v-if="field.key == 'capacity'" class="gutter-row" :span="18">
              <div class="gutter-box">{{ data[field.key] }}</div>
            </a-col>

            <a-col v-else-if="field.key == 'roleID'" class="gutter-row" :span="18">
              <div class="gutter-box">
                <a-tag :color="data.colorUserRole">
                  {{ data.textUserRole }}
                </a-tag>
              </div>
            </a-col>
            <a-col v-else-if="field.key == 'state'" class="gutter-row" :span="18">
              <div class="gutter-box">
                <a-tag :color="data.colorUserState">
                  {{ data.textUserState }}
                </a-tag>
              </div>
            </a-col>
            <a-col v-else class="gutter-row" :span="18">
              <div class="gutter-box">{{ data[field.key] }}</div>
            </a-col>
          </template>
        </a-row>
        {{ data }}
      </div>
    </div>
  </div>
</template>
<script setup>
import { userService } from "@/api";
import { ref, reactive, onBeforeMount } from "vue";
import { useRoute, useRouter } from "vue-router";
import _ from "lodash";
import { message } from "ant-design-vue";
import { ResponseCode, UserRole } from "@/constants";
import { util } from "@/utils";
const route = useRoute();
const router = useRouter();
const fields = reactive([
  {
    title: "Username",
    dataIndex: "username",
    key: "username",
  },
  {
    title: "Email",
    dataIndex: "email",
    key: "email",
  },
  {
    title: "Role",
    dataIndex: "roleID",
    key: "roleID",
  },
  {
    title: "State",
    dataIndex: "state",
    key: "state",
  },
]);
let data = ref({});
const loading = reactive({
  isLoadingBeforeMount: false,
});
onBeforeMount(async () => {
  try {
    loading.isLoadingBeforeMount = true;
    let user = await userService.getById(route.params.id);
    if (user?.success && user?.data) {
      const { colorUserState, textUserState } = util.getViewUserState(
        user.data.state
      );
      user.data.colorUserState = colorUserState;
      user.data.textUserState = textUserState;
      const { colorUserRole, textUserRole } = util.getViewUserRole(
        user.data.roleID
      );
      user.data.colorUserRole = colorUserRole;
      user.data.textUserRole = textUserRole;
      data.value = _.cloneDeep(user.data);
    }
  } catch (error) {
    console.log(error);
    switch (error.code) {
      case ResponseCode.NotFoundUser:
        message.error($t("User.NotFoundUser"));
        break;
      default:
        message.error($t("UnknownError"));
        break;
    }
    router.push({ name: "UserList" });
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const navigateEdit = () => {
  router.push({ name: "UserEdit", params: { id: route.params.id } });
}
</script>
<style lang="scss" scoped>
.container-content {
  padding: 0 16px 16px 16px;
}
</style>
