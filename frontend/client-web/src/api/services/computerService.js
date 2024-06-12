import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerService extends baseService {
   endpoint = "Computer";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
   /**
 * lấy theo computerRoomId
 * @param {*} params 
 * @returns 
 */
   async getListByComputerRoomId(computerRoomId, params) {
      return await instance.post(`${this.getEndpoint()}/GetListByComputerRoomId/${computerRoomId}`, params);
   }
   async getListFilterBySoftware(softwareId, paging) {
      return await instance.post(`${this.getEndpoint()}/GetListFilterBySoftware/${softwareId}`, paging);
   }
   async getComputerOnLineChart(checkTime) {
      return await instance.get(`${this.getEndpoint()}/GetComputerOnLineChart/${checkTime}`);
   }
   async getComputerByListErrorChart() {
      return await instance.get(`${this.getEndpoint()}/GetComputerByListErrorChart`);
   }
}
export default new ComputerService();