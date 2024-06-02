import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerSoftwareService extends baseService {
   endpoint = "ComputerSoftware";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   async getListFilterByComputer(computerId, paging) {
      return await instance.post(`${this.getEndpoint()}/GetListByComputerId/${computerId}`, paging);
   }
}
export default new ComputerSoftwareService();