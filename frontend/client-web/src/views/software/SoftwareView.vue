<template>
  <div class="container-content">
    <div class="tool-bars flex justify-between py-4 sticky top-0 -mx-4 px-4 ">
      <div class="tool-bars__left">
        <router-link :to="{ name: 'SoftwareList' }">
          <a-button shape="circle">
            <template #icon>
              <ArrowLeftOutlined />
            </template>
          </a-button>
        </router-link>

      </div>
      <div class="tool-bars__right">
        <router-link :to="{ name: 'SoftwareEdit', params: { id: route.params.id } }">
          <a-button type="primary" ghost>{{ $t("Edit") }}</a-button>
        </router-link>
      </div>
    </div>
    <div class="content pt-4">
      <div class="master">
        <h2>{{ $t("DetailInfo") }}</h2>
        <a-row :gutter="[16, 24]">
          <template v-for="field in fieldMaster" :key="field.dataIndex">
            <a-col class="gutter-row" :span="6">
              <label class="font-bold">{{ field.title }}</label>
            </a-col>

            <a-col class="gutter-row" :span="18">
              <template v-if="field.key == 'isUpdate' || field.key == 'isInstall'">
                <template v-if="dataMaster?.[field.key]">
                  <CheckCircleOutlined />
                </template>
                <template v-else>
                  <CloseCircleOutlined />
                </template>
              </template>
              <template v-else-if="field.key === 'createdAt' || field.key === 'updatedAt'">
                <span v-if="dataMaster?.[field.key]">
                  {{ moment(dataMaster[field.key]).format(FormatDateKey.Default) }}
                </span>
              </template>
              <template v-else>
                <div class="gutter-box">{{ dataMaster?.[field.key] }}</div>
              </template>
            </a-col>
          </template>
        </a-row>
      </div>
      <a-divider />
      <div class="detail">
        <h2>{{ $t("Software.FileList") }}</h2>
        <div class="detail__toolbars flex justify-between pb-4">
          <div class="detail-toolbars__left">

          </div>
          <div class="detail-toolbars__right">
            <!-- to do -->
            <a-button type="primary" @click="openFileQuickAddModal">
              <template #icon>
                <PlusOutlined />
              </template>
              {{ $t("File.AddFileTitle") }}
            </a-button>
          </div>
        </div>
        <div class="detail__content">
          <a-table :columns="columnFiles" :row-key="(record) => record.id" :row-selection="{
            selectedRowKeys: selectRows.selectedRowKeys,
            onChange: onSelectChange,
          }" :data-source="dataFiles" :pagination="false" :scroll="scrollConfig" :loading="loading.loadingTable"
            @change="handleTableChange">
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'name'">
                <router-link :to="{ name: 'SoftwareView', params: { id: record.id } }">
                  {{ record.name }}
                </router-link>
              </template>
              <template v-else-if="column.key === 'softwareName'">
                {{ record?.software?.name }}
              </template>
              <template v-else-if="column.key === 'fileSize'">
                {{ `${record?.size / 1024}` }}
                <b>KB</b>
              </template>
              <template v-else-if="column.key === 'createdAt' || column.key === 'updatedAt'">
                <span v-if="record[column.key]">
                  {{ moment(record[column.key]).format(FormatDateKey.Default) }}
                </span>
              </template>
              <template v-else-if="column.key === 'operation'">
                <div class="flex gap-2">
                  <a-button round>
                    <template #icon>
                      <EditOutlined />
                    </template>
                  </a-button>
                  <a-button round class="bg-red-200" @click="onDelete(record)">
                    <template #icon>
                      <DeleteOutlined />
                    </template>
                  </a-button>
                </div>
              </template>
            </template>
            <template #footer> {{ showTotal }} </template>
          </a-table>
        </div>

      </div>
    </div>
  </div>
  <FileQuickAddModal v-if="fileQuickAddProp.isShow" :data="fileQuickAddProp"
    @toggleShowModal="toggleShowFileQuickAddModal" @afterSave="afterSaveFile" />
  <contextHolder />
</template>
<script setup>
import { onBeforeMount, ref, reactive, computed, h } from "vue";
import { useRoute, useRouter } from "vue-router";
import _ from "lodash";
import moment from "moment";
import { Modal, message } from "ant-design-vue";
import { FormatDateKey } from "@/constants";
import { fileService, softwareService } from "@/api";
import { ExclamationCircleOutlined } from "@ant-design/icons-vue";
import FileQuickAddModal from "../file-manager/FileQuickAddModal.vue";
// ========== start state ========== 
const router = useRouter();
const route = useRoute();
const [modal, contextHolder] = Modal.useModal();
const loading = reactive({
  isLoadingBeforeMount: false,
});
const fieldMaster = reactive([
  {
    title: $t("Software.Name"),
    dataIndex: "name",
    key: "name",
  },
  {
    title: $t("Software.IsUpdate"),
    dataIndex: "isUpdate",
    key: "isUpdate",
  },
  {
    title: $t("Software.IsInstall"),
    dataIndex: "isInstall",
    key: "isInstall",
  },
  {
    title: $t("Software.CreatedAt"),
    dataIndex: "createdAt",
    key: "createdAt",
  },
  {
    title: $t("Software.UpdatedAt"),
    dataIndex: "updatedAt",
    key: "updatedAt",
  },
]);

const columnFiles = [
  {
    title: $t("File.FileName"),
    dataIndex: "fileName",
    key: "fileName",
    width: "300px",
    sorter: true,
    fixed: "left",
    ellipsis: true,
  },
  {
    title: $t("File.SoftwareName"),
    dataIndex: "softwareName",
    key: "softwareName",
    width: "160px",
  },
  {
    title: $t("File.Version"),
    dataIndex: "version",
    key: "version",
    width: "100px",
  },
  {
    title: $t("File.FileSize"),
    dataIndex: "fileSize",
    key: "fileSize",
    width: "160px",
  },
  {
    title: $t("File.CreatedAt"),
    dataIndex: "createdAt",
    key: "createdAt",
    width: "150px",
  },
  {
    title: $t("File.UpdatedAt"),
    dataIndex: "updatedAt",
    key: "updatedAt",
    width: "150px",
  },
  {
    title: "Action",
    dataIndex: "operation",
    key: "operation",
    width: "100px",
    fixed: "right",
  },
]
const dataMaster = ref({});
const dataFiles = ref([]);
const showTotal = computed(
  () => `Total ${dataFiles.value?.length || 0} items`
);
const scrollConfig = ref({ x: 1200, y: 400 });
const selectRows = reactive({
  selectedRowKeys: [],
});

const fileQuickAddProp = reactive({
  isShow: false,
});
// ========== end state ==========

// ========== start lifecycle ========== 
onBeforeMount(async () => {
  try {
    loading.isLoadingBeforeMount = true;
    await fetchDataMaster();
    await fetchDataFiles();
  } catch (error) {
    console.log(error);
    message.error($t("UnKnowError"))
  }
  finally {

  }
})
/**
 * fetch thông tin phần mềm
 */
const fetchDataMaster = async () => {
  try {
    const rsDataMaster = await softwareService.getById(route.params.id);
    if (rsDataMaster && rsDataMaster.success && rsDataMaster.data) {
      dataMaster.value = _.cloneDeep(rsDataMaster.data);
    }
  } catch (error) {
    console.log(error);
    message.error($t("UnKnowError"))
  }
}

/**
 * fetch danh sách file theo id phần mềm
 */
const fetchDataFiles = async () => {
  try {
    const rs = await fileService.getListFileBySoftwareId(route.params.id);
    if (rs && rs.success && rs.data) {
      dataFiles.value = _.cloneDeep(rs.data);
    }
  } catch (error) {
    console.log(error);
    message.error($t("UnKnowError"));
  }
}

/**
 * paging
 * @param {*} pag
 * @param {*} filters
 * @param {*} sorter
 */
const handleTableChange = async (pag, filters, sorter) => {
  console.log(
    pag.pageSize,
    pag?.current,
    sorter.field,
    sorter.order,
    filters
  );

  await fetchDataFiles();
};

/**
 * sự kiện selected rows
 * @param {*} selectedRowKeys
 */
const onSelectChange = (selectedRowKeys) => {
  console.log("selectedRowKeys changed: ", selectedRowKeys);
  selectRows.selectedRowKeys = selectedRowKeys;
};

/**
 * sự kiện tìm kiếm ở ô input search
 * @param {*} searchValue
 */
const onSearch = (searchValue) => {
  console.log("use value", searchValue);
};

/**
 * xóa bản ghi
 */
const onDelete = (record) => {
  modal.confirm({
    title: "Cảnh báo",
    icon: h(ExclamationCircleOutlined),
    content: h("div", [
      `Bạn có chắc chắn muốn xóa file ${record.fileName}.`,
    ]),
    okText: "Yes",
    okType: "danger",
    async onOk() {
      try {
        let rs = await fileService.delete(record.id);
        if (rs?.success && rs?.data) {
          message.success($t("File.DeleteSuccess", [record.fileName]));
          await fetchDataFiles();
        }
      } catch (errors) {
        console.log(errors);
        message.error($t("UnKnowError"));
      }
    },
    onCancel() { },
  });
};

const openFileQuickAddModal = () => {
  fileQuickAddProp.isShow = true;
  fileQuickAddProp.softwareId = dataMaster.value.id;
  fileQuickAddProp.softwareName = dataMaster.value.name;
}

const toggleShowFileQuickAddModal = (isShow) => {
  fileQuickAddProp.isShow = isShow;
}
const afterSaveFile = async (e) => {
  fileQuickAddProp.isShow = false;
  await fetchDataFiles();
}
// ========== end lifecycle ==========
</script>
<style lang="scss">
.container-content {
  height: 100%;

  .tool-bars {
    z-index: 1;
    background-color: #f5f5f5;
    border-bottom: 1px solid rgba(5, 5, 5, 0.06);
  }
}
</style>