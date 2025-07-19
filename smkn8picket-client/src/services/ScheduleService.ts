import axios from "axios";
import { Helper } from "@/commons";
import type Schedule from "@/models/Schedule";
import type ScheduleRequest from "@/models/Requests/ScheduleRequest";

const controller = "schedule";

const ScheduleService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  getBySchoolYearId: async (scheduleId: number) => {
    const response = await axios.get(`${controller}/byschoolyearId/${scheduleId}`);
    return Helper.getResult(response);
  },
  post: async (model: ScheduleRequest) => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: Schedule) => {
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


export default ScheduleService;
