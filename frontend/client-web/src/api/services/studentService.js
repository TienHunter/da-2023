import instance from "../instance";
import baseService from "./baseSerivce.js";
class StudentService extends baseService {
   endpoint = "Student";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
}
export default new StudentService();