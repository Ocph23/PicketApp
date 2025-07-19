import type { Pagination } from '@/models'
import {type PaginateResponse} from '@/models/Responses'
import { defineStore } from 'pinia'

const PaginationStore = defineStore('pagination', {
  // arrow function recommended for full type inference
  state: () => {
    return {
      // all these properties will have their type inferred automatically
      paginate: {},
      paginateResult: {} as PaginateResponse,
    }
  },
  actions: {
    setPaginate(paginate: Pagination) {
      this.paginate = paginate
    },
    setPaginateResult(paginateResult: PaginateResponse) {
      this.paginateResult = paginateResult
    },
  },
})


export default PaginationStore;
