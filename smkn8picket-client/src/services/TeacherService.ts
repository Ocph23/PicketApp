import axios from "axios";
import { Helper } from "@/commons";
import type { Teacher } from "@/models";

const controller = "teacher";

const TeacherService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  search: async (keyword: unknown) => {
    const response = await axios.get(`${controller}/search/${keyword}`);
    return Helper.getResult(response);
  },
  post: async (model: Teacher) => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: Teacher) => {
    const response = await axios.put(`${controller}/${id}`, model);
    return Helper.getResult(response);
  },
  delete: async (id: number) => {
    const response = await axios.delete(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  getById: async (id: number) => {
    const response = await axios.get(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  updateFoto: async (id: unknown, data: unknown) => {
    const response = await axios.put(`${controller}/photo/${id}`, data);
    return Helper.getResult(response);
  },
};



export default TeacherService;
