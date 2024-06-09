import { createRouter, createWebHistory } from "vue-router";
import routerAuth from "@/views/auth/routerAuth.js";
import routerDashboard from "../views/dashboard/router.js";
import routesComputerRoom from "../views/computer-room/route.js";
import routesComputer from "../views/computer/route.js";
import routeFile from "../views/file-manager/route.js";
import routeSoftware from "../views/software/route.js";
import routeUser from "../views/user/route.js";
import routeMonitorSession from "../views/monitor-session/route.js";
import routeConfigOption from "../views/config-option/route.js";
import routeAgent from "../views/agent/route.js";
import routeStudent from "../views/student/route.js";
import AccessDenied from "../views/AccessDenied.vue";
import LocalStorageKey from "../constants/localStorageKey.js";
import localStore from "../utils/localStore.js";

const routes = [
   ...routerAuth,
   ...routerDashboard,
   ...routesComputerRoom,
   ...routeUser,
   ...routesComputer,
   ...routeFile,
   ...routeSoftware,
   ...routeMonitorSession,
   ...routeConfigOption,
   ...routeAgent,
   ...routeStudent,
   {
      path: "/access-denied",
      name: "AccessDenied",
      component: AccessDenied
   },
   {
      path: "/:pathMatch(.*)*",
      redirect: to => {

         return { name: "Dashboard" }
      },
   },

];

const router = createRouter({
   // 4. Provide the history implementation to use. We are using the hash history for simplicity here.
   history: createWebHistory(),
   routes, // short for `routes: routes`
});

router.beforeEach((to, from, next) => {
   if (to.meta.requiresAuth) {
      // Kiểm tra xem người dùng đã đăng nhập hay chưa
      const userInfor = localStore.getItem(LocalStorageKey.userInfor);
      const accessToken = localStore.getItem(LocalStorageKey.accessToken);
      if (accessToken && userInfor) {
         if (to.meta?.permission.length > 0) {
            if (to.meta.permission.includes(userInfor.roleID)) {
               next();
            } else {
               next({ name: "NotPermission" });
            }
         } else {
            next();
         }
      } else {
         // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
         next({ name: "Login" });
      }
   } else {
      // Tuyến công khai, cho phép truy cập
      next();
   }

   // check permission
});
export default router;