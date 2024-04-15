import { createRouter, createWebHistory } from "vue-router";
import routerAuth from "@/views/auth/routerAuth.js";
import routerDashboard from "../views/dashboard/router.js";
import routesComputerRoom from "../views/computer-room/route.js";
import routesComputer from "../views/computer/route.js";
import routeUser from "../views/user/route.js";
import HelloWorld from "../components/HelloWorld.vue";
import LocalStorageKey from "../constants/localStorageKey.js";
import localStore from "../utils/localStore.js";

const routes = [
   ...routerAuth,
   ...routerDashboard,
   ...routesComputerRoom,
   ...routeUser,
   ...routesComputer,
   {
      path: "/:pathMatch(.*)*",
      name: "NotFound",
      meta: {
         title: "Không tìm thấy",
         layout: "auth",
      },
      component: HelloWorld,
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
               next({ name: "NotFound" });
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