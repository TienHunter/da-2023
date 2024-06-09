import instance from "../instance";
import baseService from "./baseSerivce.js";
class FileProofService extends baseService {
   endpoint = "FileProof";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   /**
    * lấy danh sách file theo id phiên giám sát
    * @param {*} softwareId 
    * @returns 
    */
   async getListByMonitorSessionId(monitorSessionId) {
      return await instance.get(`${this.getEndpoint()}/GetListByMonitorSessionId/${monitorSessionId}`);
   }

   async getFileByFilename(filename) {
      return await instance.get(`${this.getEndpoint()}/get-file/${filename}`, {
         responseType: "blob",
      })
   }

}
export default new FileProofService();