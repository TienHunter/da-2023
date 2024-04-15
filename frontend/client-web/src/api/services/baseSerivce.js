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

   async getById(id) {
      return await instance.get(`${this.getEndpoint()}/detail/${id}`);
   }
   async getList(params) {
      return await instance.post(`${this.getEndpoint()}/GetList`, params);
   }
}
export default BaseService;