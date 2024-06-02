import instance from "../instance";
import baseService from "./baseSerivce.js";
class ConfigOptionService extends baseService {
   endpoint = "ConfigOption";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
}
export default new ConfigOptionService();