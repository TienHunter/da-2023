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
    * update record
    * @param {*} record
    * @returns
    */
   async update(record) {
      return await instance.put(`${this.getEndpoint()}`, record);
   }

   /**
    * get all record
    * @returns
    */
   async getAll() {
      return await instance.get(`${this.getEndpoint()}`);
   }

   async getById(id) {
      return await instance.get(`${this.getEndpoint()}/${id}`);
   }
}
export default BaseService;