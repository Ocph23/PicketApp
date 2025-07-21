import axios from "axios";
import { Helper } from "@/commons";
import type { LoginRequest } from "@/models/Requests";
import type { AuthResponse } from "@/models";

const controller = "auth";

const AuthService = {
  login: async (req: LoginRequest) => {
    const response = await axios.post(`${controller}/login`, req);
    return Helper.getResult(response);
  },

  isTeacherPicket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth.isTeacherPicket;
  },

  setAdmin: async (userId: string) => {
    const response = await axios.get(`${controller}/setadmin/${userId}`);
    return Helper.getResult(response);
  },

  getTeacherId: (): number => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    if (auth.profile)
      return auth.profile.id;
    return 0;
  },
  isAdmin: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth.isAdmin;
  },
  isHomeRoomTeacher: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth.roles.includes("HomeRoomTeacher");
  },
  isPiket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth.loginAs === 'Piket';
  },

  isAdminOrPicket: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth.isAdmin || auth.isTeacherPicket;
  },
  getUser: async () => {
    const authToken = localStorage.getItem('authToken')
    const auth = JSON.parse(authToken as string) as AuthResponse;
    return auth;
  },

};


export default AuthService;
