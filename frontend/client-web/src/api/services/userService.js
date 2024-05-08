import instance from "../instance";
import baseService from "./baseSerivce.js";
class UserSerivce extends baseService {
   endpoint = "User";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   async login(userLogin) {
      return await instance.post(`${this.getEndpoint()}/login`, userLogin);
   }

   async register(userRegister) {
      return await instance.post(`${this.getEndpoint()}/register`, userRegister);
   }
   async changePassword(data) {
      return await instance.put(`${this.getEndpoint()}/ChangePassword`, data);
   }
   async logout() {
      return await instance.put(`${this.getEndpoint()}/logout`);
   }
   async checkLogin() {
      return await instance.get(`${this.getEndpoint()}/check-login`);
   }
   async updateByAdmin(data, id) {
      return await instance.put(`${this.getEndpoint()}/update-by-admin/${id}`, data);
   }
   async updateUserState(userId, userState) {
      return await instance.put(`${this.getEndpoint()}/UpdateState/${userId}/${userState}`)
   }
}
export default new UserSerivce();