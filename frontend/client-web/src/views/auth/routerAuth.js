import { LocalStorageKey } from "../../constants";
import { localStore } from "../../utils";
import PageLogin from "./PageLogin.vue";
import PageRegister from "./PageRegister.vue";
import { userService } from "@/api";
const routerAuth = [
   {
      path: "/login",
      name: "Login",
      meta: {
         layout: "auth",
         title: "Login - ComputerManagement",
      },
      component: PageLogin,
      beforeEnter: (to, from, next) => {
         let accessToken = localStore.getItem(LocalStorageKey.accessToken);
         if (accessToken) {
            next({ name: "Dashboard" });
         } else {
            next();
         }
      },
   },
   {
      path: "/register",
      name: "Register",
      meta: {
         layout: "auth",
         title: "Register - ComputerManagement",
      },
      component: PageRegister,
   },
];
export default routerAuth;
