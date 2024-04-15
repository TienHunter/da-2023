import instance from "../instance";
import baseService from "./baseSerivce.js";
class ComputerService extends baseService {
   endpoint = "Computer";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
}
export default new ComputerService();