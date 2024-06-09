import instance from "../instance";
import baseService from "./baseSerivce.js";
class ConfigOptionService extends baseService {
   endpoint = "ConfigOption";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
   async getByOptionName(optionName) {
      return await instance.get(`${this.getEndpoint()}/GetByOptionName/${optionName}`);
   }
}
export default new ConfigOptionService();