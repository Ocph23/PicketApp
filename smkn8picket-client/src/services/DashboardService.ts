import axios from "axios"
import { Helper } from '@/commons'

const controller = "dashboard";

const DashboardService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },
}


export default DashboardService;
