import MonitorSessionList from "./MonitorSessionList.vue";
import MonitorSessionEdit from "./MonitorSessionEdit.vue";
import MonitorSessionView from "./MonitorSessionView.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/monitor-session",
      name: "MonitorSession",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "MonitorSessionList" }
      },

      children: [
         {
            path: "list",
            name: "MonitorSessionList",
            component: MonitorSessionList,
         },
         {
            path: "add",
            name: "MonitorSessionAdd",
            component: MonitorSessionEdit,
            meta: {
               formMode: FormMode.Add
            }
         },
         {
            path: ":id/edit",
            name: "MonitorSessionEdit",
            component: MonitorSessionEdit,
            meta: {
               formMode: FormMode.Update
            }
         },
         {
            path: ":id/view",
            name: "MonitorSessionView",
            component: MonitorSessionView,
         },
      ],
   },
];
export default routes;