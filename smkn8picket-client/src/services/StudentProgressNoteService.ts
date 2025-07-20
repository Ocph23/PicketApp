import axios from "axios";
import { Helper } from "@/commons";
import type StudentProgressNoteRequest from "@/models/Requests/StudentProgressNoteRequest";

const controller = "studentprogressnote";

const StudentProgressNoteService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  getByStudentId: async (studentId: number) => {
    const response = await axios.get(`${controller}/bystudentid/${studentId}`);
    return Helper.getResult(response);
  },
  put: async (id: number, model: StudentProgressNoteRequest) => {
    const response = await axios.put(`${controller}/${id}`, model);
    return Helper.getResult(response);
  },
  create: async (data: StudentProgressNoteRequest) => {
    const response = await axios.post(`${controller}`, data);
    return Helper.getResult(response);
  },
  delete: async (id: string) => {
    const response = await axios.delete(`${controller}/${id}`);
    return Helper.getResult(response);
  },
};

export default StudentProgressNoteService;
