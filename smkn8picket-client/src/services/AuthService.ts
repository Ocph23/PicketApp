import axios from "axios";
import { Helper } from "@/commons";
import type { LoginRequest } from "@/models/Requests";
import type { AuthResponse } from "@/models";

const controller = "auth";

const AuthService = {
  login: async (req: LoginRequest) => {
    const response = await axios.post(`${controller}/login`, req)
    return Helper.getResult(response)
  },

  resetpasswordByAdmin: async (userId: string) => {
    const response = await axios.get(`${controller}/resetpassword/${userId}`)
    return Helper.getResult(response)
  },

  resetpassword: async () => {
    const response = await axios.get(`${controller}/resetpassword`)
    return Helper.getResult(response)
  },


  changePassword: async (userName: string, resetToke: string, newPassword: string) => {
    const data = { userName, resetToke, newPassword };
    const response = await axios.post(`${controller}/changepassword`, data);
    return Helper.getResult(response)
  },

  isTeacherPicket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth.isTeacherPicket
  },

  setAdmin: async (userId: string) => {
    const response = await axios.get(`${controller}/setadmin/${userId}`)
    return Helper.getResult(response)
  },
  removeAsAdmin: async (userId: string) => {
    const response = await axios.get(`${controller}/removeasadmin/${userId}`)
    return Helper.getResult(response)
  },

  lockUser: async (userId: string) => {
    const response = await axios.get(`${controller}/lockuser/${userId}`)
    return Helper.getResult(response)
  },
  unlockUser: async (userId: string) => {
    const response = await axios.get(`${controller}/unlockuser/${userId}`)
    return Helper.getResult(response)
  },

  getTeacherId: (): number => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    if (auth.profile) return auth.profile.id
    return 0
  },

  getStatus: async (userId: string) => {
    const response = await axios.get(`${controller}/status/${userId}`)
    return Helper.getResult(response)
  },

  isAdmin: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth.isAdmin
  },
  isHomeRoomTeacher: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth.roles.includes('HomeRoomTeacher')
  },
  isPiket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth.loginAs === 'Piket'
  },

  isAdminOrPicket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth.isAdmin || auth.isTeacherPicket
  },
  getUser: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse
    return auth
  },
}


export default AuthService;
