<template>
  <div class="container-content">
    <div class="toolbars flex justify-between bg-gray-200">
      <div class="toolbar-left">
        <router-link :to="{ name: 'ComputerList' }">
          <a-button shape="circle" size="small" class="mr-2">
            <template #icon>
              <ArrowLeftOutlined />
            </template>
          </a-button>
        </router-link>
      </div>
      <div class="toolbar-right">
        <a-button type="primary" ghost v-has-permission="`${UserRole.Admin}`"
          v-passPermissionClick="() => navigateEdit()">{{
            $t("Edit") }}</a-button>

      </div>
    </div>
    <div class="content">
      <div class="master">
        <h2>{{ $t("DetailInfo") }}</h2>
        <a-row :gutter="[16, 24]">
          <template v-for="field in fields" :key="field.dataIndex">
            <a-col class="gutter-row" :span="6">
              <label class="font-bold">{{ field.title }}</label>
            </a-col>
            <a-col v-if="field.key == 'state'" class="gutter-row" :span="18">
              <div class="gutter-box">
                <a-tag :color="computerInfo.colorComputerState">
                  {{ computerInfo.textComputerState }}
                </a-tag>
              </div>
            </a-col>
            <a-col v-else-if="field.key == 'listError'" class="gutter-row" :span="18">
              <div class="gutter-box">
                <a-tag v-for="(tag, index) in computerInfo.listError" :key="index" :color="tag.color">
                  {{ tag.label }}
                </a-tag>
              </div>
            </a-col>
            <a-col v-else class="gutter-row" :span="18">
              <div class="gutter-box">{{ computerInfo[field.key] }}</div>
            </a-col>
          </template>
        </a-row>
        <a-divider />
        <div class="detail">
          <h2>{{ $t("Computer.ListSoftware") }}</h2>
          <div class="detail__toolbars flex justify-between pb-4">
            <div class="detail-toolbars__left">

            </div>
            <div class="detail-toolbars__right">
              <!-- to do -->
            </div>
          </div>
          <div class="detail__content">
            <a-table :columns="columnsComputerSoftware" :data-source="dataComputerSoftwares" :pagination="false"
              :loading="loading.loadingTable">
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'softwareName'">
                  <router-link :to="{ name: 'SoftwareView', params: { id: record.softwareId } }">
                    {{ record?.software?.name }}
                  </router-link>
                </template>
                <template v-else-if="column.key === 'isDowloadFile'">
                  <template v-if="record?.isDowloadFile">
                    <CheckCircleOutlined style="color:green" />
                  </template>
                  <template v-else>
                    <StopOutlined />
                  </template>
                </template>
                <template v-else-if="column.key === 'isInstalled'">
                  <template v-if="record?.isInstalled">
                    <CheckCircleOutlined style="color:green" />
                  </template>
                  <template v-else>
                    <StopOutlined />
                  </template>
                </template>

                <template v-else-if="column.key === 'operation'">
                  <div class="flex gap-2">
                    <a-dropdown :trigger="['click']">
                      <template #overlay>
                        <a-menu>
                          <a-menu-item
                            @click="() => updateCommandOptionDowloadFile(record, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, true)">
                            <CheckCircleOutlined />
                            Bật tự dộng tải/cập nhật file cài đặt
                          </a-menu-item>
                          <a-menu-item
                            @click="() => updateCommandOptionDowloadFile(record, CommandOptionKey.CHECK_DOWLOAD_SOFTWARE, false)">
                            <StopOutlined />
                            Tắt tự động tải/ cập nhật file cài dặt
                          </a-menu-item>
                          <a-menu-item
                            @click="() => updateCommandOptionDowloadFile(record, CommandOptionKey.CHECK_INSTALL_SOFTWARE, true)">
                            <StopOutlined />
                            Bật kiểm tra cài dặt phần mềm
                          </a-menu-item>
                          <a-menu-item
                            @click="() => updateCommandOptionDowloadFile(record, CommandOptionKey.CHECK_INSTALL_SOFTWARE, false)">
                            <StopOutlined />
                            Tắt kiểm tra cài dặt phần mềm
                          </a-menu-item>
                        </a-menu>
                      </template>
                      <a-button round>
                        <template #icon>
                          <MoreOutlined />
                        </template>
                      </a-button>
                    </a-dropdown>
                  </div>
                </template>
              </template>
            </a-table>
          </div>

        </div>
      </div>
    </div>

  </div>
</template>
<script setup>
import { commandOptionService, computerService, computerSoftwareService, userService } from "@/api";
import { ref, reactive, onBeforeMount, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import moment from "moment";
import _ from "lodash";
import { message } from "ant-design-vue";
import { ResponseCode, FormatDateKey, ComputerKey, UserRole, CommonKey, CommandOptionKey, LocalStorageKey } from "@/constants";
import { localStore, util } from "@/utils";
const route = useRoute();
const router = useRouter();
const fields = reactive([
  {
    title: $t("Computer.Name"),
    dataIndex: "name",
    key: "name",
  },
  {
    title: $t("Computer.ComputerRoomName"),
    dataIndex: "computerRoomName",
    key: "computerRoomName",
  },
  {
    title: $t("Computer.MacAddress"),
    dataIndex: "macAddress",
    key: "macAddress",
  },
  {
    title: $t("Computer.Row"),
    dataIndex: "row",
    key: "row",
  },
  {
    title: $t("Computer.Col"),
    dataIndex: "col",
    key: "col",
  },
  {
    title: $t("Computer.OS"),
    dataIndex: "os",
    key: "os",
  },
  {
    title: $t("Computer.CPU"),
    dataIndex: "cpu",
    key: "cpu",
  },
  {
    title: $t("Computer.RAM"),
    dataIndex: "ram",
    key: "ram",
  },
  {
    title: $t("Computer.HardDriver"),
    dataIndex: "hardDriver",
    key: "hardDriver",
  },
  {
    title: $t("Computer.Condition"),
    dataIndex: "listError",
    key: "listError",
  },
]);
let computerInfo = ref({});
const loading = reactive({
  isLoadingBeforeMount: false,
  loadingTable: false
});

const columnsComputerSoftware = computed(() => {
  return [
    {
      title: $t("Software.Name"),
      dataIndex: "softwareName",
      key: "softwareName",
      width: "200px",
      fixed: "left",
      ellipsis: true,
    },
    {
      title: $t("Computer.IsDowloaded"),
      dataIndex: "isDowloadFile",
      key: "isDowloadFile",
      width: "80px",
    },
    {
      title: $t("Computer.IsInstalled"),
      dataIndex: "isInstalled",
      key: "isInstalled",
      width: "80px",
    },
    {
      title: $t("Action"),
      dataIndex: "operation",
      key: "operation",
      width: "100px",
      fixed: "right",
    },
  ]
})
const dataComputerSoftwares = ref([]);
onBeforeMount(async () => {
  try {
    loading.isLoadingBeforeMount = true;
    let computer = _.cloneDeep(route.meta.data);
    if (computer) {
      computer.listError = computer?.listErrorId?.length > 0 ? computer.listErrorId.map(errorId => {
        const { label, color } = util.handleRenderComputerError(errorId);
        return {
          value: errorId,
          label: label,
          color: color
        }
      }) : [{ value: ComputerKey.ComputerError.Perfect, label: $t("Computer.ComputerError.Perfect"), color: 'green' }]
      computer.computerRoomName = computer.computerRoom?.name;
      computerInfo.value = computer;

      // láy danh sách phần mềm
      await getListComputerSoftware();
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
    router.push({ name: "ComputerList" });
  } finally {
    loading.isLoadingBeforeMount = false;
  }
});

const navigateEdit = () => {
  router.push({ name: "ComputerEdit", params: { id: route.params.id } })
}


const getListComputerSoftware = async () => {
  try {
    loading.loadingTable = true
    const rs = await computerSoftwareService.getListFilterByComputer(computerInfo.value.id, {});
    if (rs && rs.data) {
      dataComputerSoftwares.value = rs.data;
    }
  } catch (error) {
    console.log(error);
  } finally {
    loading.loadingTable = false;
  }
}
/**
 * tạo commandOption và cập nhật
 * @param record 
 * @param key 
 * @param value 
 */
const updateCommandOptionDowloadFile = async (record, keyOption, value) => {
  const userInfor = localStore.getItem(LocalStorageKey.userInfor);
  if (!(userInfor && userInfor.roleID == UserRole.Admin)) {
    message.warning($t("NotPermission"));
    return;
  }
  let commandOption = {
    sourceIds: [record.computerId],
    desId: record.softwareId,
    commandKey: keyOption,
    keyMapping: CommonKey.COMPUTER,
    commandValue: value
  };

  try {
    let rs = await commandOptionService.upsertCommand(commandOption)
    if (rs && rs.success) {
      message.success($t("SaveSuccess"));
    }
  } catch (error) {
    console.log(error);
    message.error($t("UnknownError"));
  }
};

</script>
<style></style>

<style lang="scss" scoped>
.container-content {
  overflow: auto;
  position: relative;
  padding: 0;

  .toolbars {
    position: sticky;
    top: 0;
    padding: 16px;
    z-index: 1;

  }

  .content {
    padding: 0 16px 16px 16px;
  }
}
</style>
