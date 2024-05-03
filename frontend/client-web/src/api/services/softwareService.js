import instance from "../instance";
import baseService from "./baseSerivce.js";
class SoftwareService extends baseService {
   endpoint = "Software";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
}
export default new SoftwareService();