import ConfigOptionList from "./ConfigOptionList.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/config-option",
      name: "ConfigOption",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "ConfigOptionList" }
      },

      children: [
         {
            path: "list",
            name: "ConfigOptionList",
            component: ConfigOptionList,
         },
      ],
   },
];
export default routes;