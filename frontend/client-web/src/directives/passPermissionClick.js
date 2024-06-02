import { message } from "ant-design-vue";

export default {
   mounted(el, binding) {
      const userInfor = JSON.parse(localStorage.getItem("userInfor"));
      el.addEventListener('click', (event) => {
         // Lấy giá trị permissions từ dataset
         const permissions = el.dataset.permissions?.split(';');

         if (userInfor && permissions?.length) {
            const hasPermission = permissions.some(p => p == userInfor.roleID);
            if (hasPermission) {
               // Gọi phương thức Vue được truyền vào sự kiện click
               binding.value(event);
            } else {
               message.warning($t("NotPermission"));
            }
         } else {
            message.warning($t("NotPermission"));
         }
      });
   },
   beforeUnmount(el) {
      el.removeEventListener('click', () => { });
   }
};