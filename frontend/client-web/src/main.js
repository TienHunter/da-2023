import { createApp } from 'vue'
import Antd from "ant-design-vue";
import { notification, message } from "ant-design-vue";
import * as icons from "@ant-design/icons-vue";
import Icon from "@ant-design/icons-vue";
import "ant-design-vue/dist/reset.css";
import './style.css'
import App from './App.vue'
import router from "./routers";
import i18n from "./i18n";
import { registerGlobalComponents } from "./utils/import";
const app = createApp(App);
registerGlobalComponents(app);
app.use(router);
app.use(Antd);
app.use(i18n);
app.component("icon", Icon);
// Tạo thành phần Vue cho mỗi biểu tượng
for (const key in icons) {
   app.component(key, icons[key]);
}
message.config({
   top: "100px",
   duration: 2,
   maxCount: 3,
   rtl: true,
   prefixCls: "toast-message",
});
app.mount("#app");