import StudentList from "./StudentList.vue";
import { Permission, FormMode } from "../../constants";
const routes = [
   {
      path: "/student",
      name: "Student",
      meta: {
         layout: "default",
         requiresAuth: true, // Indicate that authentication is required for this route
         permission: [Permission.Admin, Permission.Teacher]
      },
      redirect: to => {
         // the function receives the target route as the argument
         // we return a redirect path/location here.
         return { name: "StudentList" }
      },

      children: [
         {
            path: "list",
            name: "StudentList",
            component: StudentList,
         },
      ],
   },
];
export default routes;