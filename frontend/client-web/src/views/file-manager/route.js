import FileList from "./FileList.vue";
import FileEdit from "./FileEdit.vue";
import FileView from "./FileView.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/file",
      name: "File",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "FileList" }
      },

      children: [
         {
            path: "list",
            name: "FileList",
            component: FileList,
         },
         {
            path: "add",
            name: "FileAdd",
            component: FileEdit,
            meta: {
               formMode: FormMode.Add
            }
         },
         {
            path: ":id/edit",
            name: "FileEdit",
            component: FileEdit,
            meta: {
               formMode: FormMode.Update
            }
         },
         {
            path: ":id/view",
            name: "FileView",
            component: FileView,
         },
      ],
   },
];
export default routes;