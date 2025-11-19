import axios from "axios";
import { Helper } from "@/commons";
import type { Pagination, Picket, } from "@/models";
import type DailyJournal from "@/models/DailyJournal";
import type LateAndComeHomeEarlyRequest from "@/models/LateAndComeHomeEarly";


const controller = "picket";

const PicketService = {
  get: async () => {
    const response = await axios.get(`${controller}`);
    return Helper.getResult(response);
  },

  getById: async (id: number) => {
    const response = await axios.get(`${controller}/${id}`);
    return Helper.getResult(response);
  },
  Pageninate: async (model: Pagination) => {
    const response = await axios.post(`${controller}/paginate`, model);
    return Helper.getResult(response);
  },

  openPicket: async () => {
    const response = await axios.post(`${controller}`, {});
    return Helper.getResult(response);

  },
  putPicket: async (id: number, picket: Picket) => {
    const response = await axios.put(`${controller}/${id}`, picket);
    return Helper.getResult(response);
  },

  postJournal: async (model: DailyJournal) => {
    const response = await axios.post(`${controller}/deilyjournal`, model);
    return Helper.getResult(response);
  },
  deleteJournal: async (id: number) => {
    const response = await axios.delete(`${controller}/deilyjournal/${id}`);
    return Helper.getResult(response);
  },
  putJournal: async (id: number, model: DailyJournal) => {
    const response = await axios.put(`${controller}/deilyjournal/${id}`, model);
    return Helper.getResult(response);
  },
  addLateOrComeHomeEarly: async (model: LateAndComeHomeEarlyRequest) => {
    const response = await axios.post(`${controller}/lateandearly`, model);
    return Helper.getResult(response);
  },

  deleteLateOrComeHomeEarly: async (id: number) => {
    const response = await axios.delete(`${controller}/lateandearly/${id}`);
    return Helper.getResult(response);
  },


  ///absne



};


export default PicketService;
