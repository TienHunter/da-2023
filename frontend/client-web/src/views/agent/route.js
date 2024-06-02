
import AgentView from "./AgentView.vue";
import { Permission, FormMode } from "../../constants";
import { agentService } from "@/api";
const routes = [
   {
      path: "/agent",
      name: "Agent",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin]
      },
      component: AgentView,
      beforeEnter: async (to, from, next) => {
         try {
            const rs = await agentService.getFirst();
            if (rs && rs.success) {
               to.meta.data = rs.data
               next();
            } else {
               next();
            }
         } catch (error) {
            console.log(error);
            message.error($t("UnknownError"));
            next();
         } finally {

         }
      }
   },
];
export default routes;