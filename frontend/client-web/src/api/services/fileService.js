import instance from "../instance";
import baseService from "./baseSerivce.js";
class FileService extends baseService {
   endpoint = "File";
   constructor() {
      super(); // Gọi constructor của lớp cha
      // Các định nghĩa riêng của lớp con...
   }

   /**
    * thêm file cài đặt
    * @param {*} formData 
    * @returns 
    */
   async uploadFile(formData) {
      return await instance.post(`${this.getEndpoint()}/upload-file`, formData)
   }

   /**
    * lấy danh sách file theo id phần mềm
    * @param {*} softwareId 
    * @returns 
    */
   async getListFileBySoftwareId(softwareId) {
      return await instance.get(`${this.getEndpoint()}/GetListFileBySoftwareId/${softwareId}`);
   }

}
export default new FileService();