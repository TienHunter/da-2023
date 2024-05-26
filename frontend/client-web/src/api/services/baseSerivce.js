import instance from "../instance";

class BaseService {
   constructor() { };
   endpoint = "";

   getEndpoint() {
      return this.endpoint;
   }

   /**
 * add new record
 * @param {*} record
 * @returns
 */
   async add(record) {
      return await instance.post(`${this.getEndpoint()}`, record);
   };

   /**
    * 
    * @param {*} record 
    * @param {*} id 
    * @returns 
    */
   async update(record, id) {
      return await instance.put(`${this.getEndpoint()}/update/${id}`, record);
   }

   /**
    * get all record
    * @returns
    */
   async getAll() {
      return await instance.get(`${this.getEndpoint()}`);
   }

   /**
    * lấy bản ghi theo id
    * @param {*} id 
    * @returns 
    */
   async getById(id) {
      return await instance.get(`${this.getEndpoint()}/detail/${id}`);
   }

   /**
    * lấy theo phân trang
    * @param {*} params 
    * @returns 
    */
   async getList(params = {}) {
      return await instance.post(`${this.getEndpoint()}/GetList`, params);
   }

   /**
    * xóa 1 bản ghi
    * @param {*} recordId 
    * @returns 
    */
   async delete(recordId) {
      return await instance.delete(`${this.getEndpoint()}/delete/${recordId}`);
   }

   /**
    * xóa nhiều bản ghi
    * @param {*} recordIds 
    * @returns 
    */
   async deleteRange(recordIds) {
      return await instance.post(`${this.getEndpoint()}/deletes`, recordIds);
   }
}
export default BaseService;