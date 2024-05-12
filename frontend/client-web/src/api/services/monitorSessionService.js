import instance from "../instance";
import baseService from "./baseSerivce.js";
class MonitorSessionService extends baseService {
   endpoint = "MonitorSession";
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
}
export default new MonitorSessionService();