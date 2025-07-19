import axios from "axios";
import { Helper } from "@/commons";
import type { Pagination, Student } from "@/models";

const controller = "student";

const StudentService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  Pageninate: async (model: Pagination) => {
    const response = await axios.post(`${controller}/paginate`, model);
    return Helper.getResult(response);
  },
  search: async (keyword: unknown) => {
    const response = await axios.get(`${controller}/search/${keyword}`);
    return Helper.getResult(response);
  },
  post: async (model: Student) => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: Student) => {
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
  getStudentWithClassById: async (id: number) => {
    const response = await axios.get(`${controller}/withclas/${id}`);
    return Helper.getResult(response);
  },
  updateFoto: async (id: number, data: string) => {
    const response = await axios.put(`/${controller}/uploadphoto/${id}`, data);
    return Helper.getResult(response);
  },
};


export default StudentService;
