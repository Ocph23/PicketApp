import axios from "axios";
import { Helper, type RequestResponse } from "@/commons";
import type { Department } from "@/models";

const controller = "department";

const DepartmentService = {
  get: async (): Promise<RequestResponse> => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  getById: async (id: number): Promise<RequestResponse> => {
    const response = await axios.get(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  post: async (model: Department): Promise<RequestResponse> => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: Department): Promise<RequestResponse> => {
    const response = await axios.put(`${controller}/${id}`, model);
    return Helper.getResult(response);
  },
  delete: async (id: number): Promise<RequestResponse> => {
    const response = await axios.delete(`${controller}/${id}`);
    return Helper.getResult(response);
  },
};
export default DepartmentService;
