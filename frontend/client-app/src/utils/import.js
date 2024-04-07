import { defineAsyncComponent } from "vue";

export function registerGlobalComponents(app) {
   app.component(
      "auth-layout",
      defineAsyncComponent(() => import("../views/layouts/AuthLayout.vue"))
   );
   app.component(
      "default-layout",
      defineAsyncComponent(() => import("../views/layouts/DefaultLayout.vue"))
   );
}
