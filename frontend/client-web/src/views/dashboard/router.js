import PageDashboard from "./PageDashboard.vue";
import { Permission } from "../../constants";
const routerDashboard = [
   {
      path: "",
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "Dashboard" }
      },
   },
   {
      path: "/dashboard",
      name: "Dashboard",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      component: PageDashboard,

   },
];
export default routerDashboard;
