import axios from 'axios'
import { Helper } from '@/commons'
import type TeacherAttendanceRequest from '@/models/Requests/TeacherAttendanceRequest'

const controller = 'teacherattendance'

const TeacherAttendanceService = {
  get: async (month: number, year: number) => {
    const response = await axios.get(`${controller}/${month}/${year}`)
    return Helper.getResult(response)
  },
  getByTeacherId: async (schoolYearId: number, teacherId: number) => {
    const response = await axios.get(`${controller}/bystudentid/${schoolYearId}/${teacherId}`)
    return Helper.getResult(response)
  },
  create: async (data: TeacherAttendanceRequest) => {
    const response = await axios.post(`${controller}`, data)
    return Helper.getResult(response)
  },
  delete: async (id: string) => {
    const response = await axios.delete(`${controller}/${id}`)
    return Helper.getResult(response)
  },
}

export default TeacherAttendanceService
