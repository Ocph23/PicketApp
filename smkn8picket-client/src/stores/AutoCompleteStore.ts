import type { AutoCompleteSuggestion, } from '@/models'

import { defineStore } from 'pinia'

const AutoCompleteStore = defineStore('autocomplete', {
  // arrow function recommended for full type inference
  state: () => {
    return {
      // all these properties will have their type inferred automatically
      teachers: [] as AutoCompleteSuggestion[],
      students: [] as AutoCompleteSuggestion[],
    }
  },
  actions: {
    setTeacher(data: AutoCompleteSuggestion[]) {
      this.teachers = data
    },
    setStudent(data: AutoCompleteSuggestion[]) {
      this.students = data
    }
  },
})


export default AutoCompleteStore;
