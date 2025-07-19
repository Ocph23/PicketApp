import axios from "axios"
import { Helper } from '@/commons'
import type { ClassRoomRequest } from '@/models'
import type ClassRoomFromLastClassRequest from "@/models/Requests/ClassRoomFromLastClassRequest";

const controller = "classroom";

const ClassRoomService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
  getById: async (id: number) => {
    const response = await axios.get(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  getBySchoolYearId: async (id: number) => {
    const response = await axios.get(`${controller}/schoolyear/${id}`);
    return Helper.getResult(response);
  },
  getByTeacher: async () => {
    const response = await axios.get(`${controller}/byteacher`);
    return Helper.getResult(response);
  },
  post: async (model: ClassRoomRequest) => {
    const response = await axios.post(`${controller}`, model);
    return Helper.getResult(response);
  },
  addStudentToClass: async (classroomId: number, model: unknown) => {
    const response = await axios.post(`${controller}/addstudent/${classroomId}`, model);
    return Helper.getResult(response);
  },
  put: async (id: number, model: ClassRoomRequest) => {
    const response = await axios.put(`${controller}/${id}`, model);
    return Helper.getResult(response);
  },
  delete: async (id: number) => {
    const response = await axios.delete(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  removeStudent: async (classroomId: number, studentId: number) => {
    const response = await axios.delete(`${controller}/removestudent/${classroomId}/${studentId}`);
    return Helper.getResult(response);
  },
  createNewClassFromClassRoom: async (model: ClassRoomFromLastClassRequest) => {
    const response = await axios.post(`${controller}/classroomfromlast`, model);
    return Helper.getResult(response);
  },
}


export default ClassRoomService;
