export default {
   mounted(el, binding) {
      // Lưu trữ key permission trên element để sử dụng trong passPermissionClick
      el.dataset.permissions = binding.value;
   },
   beforeUnmount(el) {
      // Xóa key permissions khỏi element trước khi component bị huỷ bỏ
      delete el.dataset.permissions;
   }
};