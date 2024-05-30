// src/api/axios.js
import axios from "axios";
import router from "@/routers";
import { localStore } from "../utils";
import { LocalStorageKey } from "../constants";
const BASE_URL = "https://localhost:44313/api-web/";
const instance = axios.create({
   baseURL: BASE_URL, // Thay thế bằng URL của API thực tế
   // Các cấu hình khác của Axios
   // headers: {
   //   "Access-Control-Allow-Origin": "*", // Hoặc chỉ định nguồn gốc cụ thể của bạn
   // },
});

instance.interceptors.request.use(
   (config) => {
      const accessToken = localStore.getItem(LocalStorageKey.accessToken);
      if (accessToken) {
         config.headers["Authorization"] = "Bearer " + accessToken;
      }

      return config;
   },
   (error) => {
      return Promise.reject(error);
   }
);
instance.interceptors.response.use(
   (response) => {
      return response.data;
   },
   (error) => {
      //  console.log(error);
      // Xử lý lỗi phát sinh trong quá trình gửi yêu cầu hoặc nhận phản hồi
      const { status, data } = error?.response ?? {};
      if (status === 401) {
         // Xử lý đăng xuất ở đây
         // xóa token đi
         localStore.clear();
         router.push({ name: "Login", params: {} });
      } else {
         return Promise.reject(error.response.data);
      }
   }
);

export default instance;
