import axios from "axios";
import { Helper } from "@/commons";
import type { SchoolYear } from "@/models";

const controller = "schoolyear";

const SchoolYearService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  getActive: async () => {
    const response = await axios.get(`${controller}/active`);
    return Helper.getResult(response);
  },
  post: async (model: SchoolYear) => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: SchoolYear) => {
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
};

export default SchoolYearService;
