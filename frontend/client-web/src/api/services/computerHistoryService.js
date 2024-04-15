import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerHistoryService extends baseService {
   endpoint = "ComputerHistory";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   async getListByComputerId(computerId, paging) {
      return await instance.post(`${this.getEndpoint()}/GetListByComputerId/${computerId}`, paging);
   }
}
export default new ComputerHistoryService();