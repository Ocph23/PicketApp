import axios from "axios";
import { Helper } from "@/commons";
import type StudentAttendanceRequest from "@/models/Requests/StudentAttendanceRequest";

const controller = "studentattendance";

const StudentAttendanceService = {
  get: async (classroom: number, month: number, year: number) => {
    const response = await axios.get(`${controller}/${classroom}/${month}/${year}`);
    return Helper.getResult(response);
  },
  getByStudentId: async (schoolYearId: number, studentId: number) => {
    const response = await axios.get(`${controller}/bystudentid/${schoolYearId}/${studentId}`);
    return Helper.getResult(response);
  },
  create: async (data: StudentAttendanceRequest) => {
    const response = await axios.post(`${controller}`, data);
    return Helper.getResult(response);
  },
  delete: async (id: string) => {
    const response = await axios.delete(`${controller}/${id}`);
    return Helper.getResult(response);
  },
};

export default StudentAttendanceService;
