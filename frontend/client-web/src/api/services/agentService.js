import instance from "../instance";
import baseService from "./baseSerivce.js";
class AgentService extends baseService {
   endpoint = "Agent";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }
   async getFirst() {
      return await instance.get(`${this.getEndpoint()}/GetFirst`);
   }

   async upsertAgent(formData) {
      return await instance.post(`${this.getEndpoint()}/UpsertAgent`, formData)
   }

   async getFile() {
      return await instance.get(`${this.getEndpoint()}/GetFile`, {
         responseType: "blob",
      })
   }
}
export default new AgentService();