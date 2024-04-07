import ComputerRoomList from "./ComputerRoomList.vue";
import ComputerRoomEdit from "./ComputerRoomEdit.vue";
import ComputerRoomView from "./ComputerRoomView.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/computer-room",
      name: "ComputerRoom",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "ComputerRoomList" }
      },

      children: [
         {
            path: "list",
            name: "ComputerRoomList",
            component: ComputerRoomList,
         },
         {
            path: "add",
            name: "ComputerRoomAdd",
            component: ComputerRoomEdit,
            meta: {
               formMode: FormMode.Add
            }
         },
         {
            path: ":id/edit",
            name: "ComputerRoomEdit",
            component: ComputerRoomEdit,
            meta: {
               formMode: FormMode.Update
            }
         },
         {
            path: ":id/view",
            name: "ComputerRoomView",
            component: ComputerRoomView,
         },
      ],
   },
];
export default routes;