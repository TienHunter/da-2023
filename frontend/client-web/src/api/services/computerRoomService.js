import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerRoomService extends baseService {
   endpoint = "ComputerRoom";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   async getListFilterBySoftware(softwareId, paging) {
      return await instance.post(`${this.getEndpoint()}/GetListFilterBySoftware/${softwareId}`, paging);
   }
}
export default new ComputerRoomService();