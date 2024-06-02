import ComputerList from "./ComputerList.vue";
import ComputerView from "./ComputerView.vue";
import ComputerEdit from "./ComputerEdit.vue";
import ComputerAdd from "./ComputerAdd.vue";
import { Permission, FormMode } from "../../constants";
import ComputerDetailView from "./ComputerDetailView.vue";
import ComputerHistoryView from "./ComputerHistoryView.vue";
import ComputerNoteView from "./ComputerNoteView.vue";
import { computerService } from "@/api";
const routes = [
   {
      path: "/computer",
      name: "Computer",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "ComputerList" }
      },

      children: [
         {
            path: "list",
            name: "ComputerList",
            component: ComputerList,
         },
         {
            path: ":id/view",
            name: "ComputerView",
            component: ComputerView,
            beforeEnter: async (to, from, next) => {
               let isSuccess = false;
               try {
                  const rs = await computerService.getById(to.params.id);
                  if (rs && rs.success && rs.data) {
                     to.meta.data = rs.data
                     isSuccess = true;
                     next();
                  } else {
                     next({ name: "ComputerList" });
                  }
               } catch (error) {
                  console.log(error);
                  message.error($t("UnknownError"));
                  next({ name: "ComputerList" });
               } finally {

               }
            }
         },
         {
            path: ":id/edit",
            name: "ComputerEdit",
            component: ComputerEdit,
            meta: {
               formMode: FormMode.Update
            }
         },
         {
            path: "add",
            name: "ComputerAdd",
            component: ComputerEdit,
            meta: {
               formMode: FormMode.AddMode
            }
         },
      ],
   },
];
export default routes;