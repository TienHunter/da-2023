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
   async getListByComputerRoomId(computerId, params) {
      return await instance.post(`${this.getEndpoint()}/GetListByComputerRoomId/${computerId}`, params);
   }
}
export default new ComputerService();