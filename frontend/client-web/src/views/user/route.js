import UserList from "./UserList.vue";
import UserView from "./UserView.vue";
import UserEdit from "./UserEdit.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/user",
      name: "User",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "UserList" }
      },

      children: [
         {
            path: "list",
            name: "UserList",
            component: UserList,
         },
         {
            path: ":id/view",
            name: "UserView",
            component: UserView,
         },
         {
            path: ":id/edit",
            name: "UserEdit",
            component: UserEdit,
            meta: {
               formMode: FormMode.Update,
            },
         },
      ],
   },
];
export default routes;