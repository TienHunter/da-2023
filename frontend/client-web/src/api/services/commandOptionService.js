import instance from "../instance";
import baseService from "./baseSerivce.js";
class CommandOptionService extends baseService {
   endpoint = "CommandOption";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   async upsertCommand(commandOption) {
      return await instance.post(`${this.getEndpoint()}/Upsert`, commandOption);
   }
}
export default new CommandOptionService();