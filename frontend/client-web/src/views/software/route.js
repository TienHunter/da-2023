import SoftwareList from "./SoftwareList.vue";
import SoftwareEdit from "./SoftwareEdit.vue";
import SoftwareView from "./SoftwareView.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/software",
      name: "Software",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "SoftwareList" }
      },

      children: [
         {
            path: "list",
            name: "SoftwareList",
            component: SoftwareList,
         },
         {
            path: "add",
            name: "SoftwareAdd",
            component: SoftwareEdit,
            meta: {
               formMode: FormMode.Add
            }
         },
         {
            path: ":id/edit",
            name: "SoftwareEdit",
            component: SoftwareEdit,
            meta: {
               formMode: FormMode.Update
            }
         },
         {
            path: ":id/view",
            name: "SoftwareView",
            component: SoftwareView,
         },
      ],
   },
];
export default routes;