import { createApp } from "vue";
import App from "./App.vue";
import { createPinia } from "pinia";
import router from "./router";
import "./assets/css/tailwind.css";
// import "./assets/css/tailwindop.css";

createApp(App).use(router).use(createPinia()).mount("#app");
