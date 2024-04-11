import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerRoomService extends baseService {
   endpoint = "ComputerRoom";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
}
export default new ComputerRoomService();