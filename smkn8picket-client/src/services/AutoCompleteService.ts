import type { AutoCompleteSuggestion, Student, Teacher } from "@/models";
import StudentService from "./StudentService";
import TeacherService from "./TeacherService";

const AutoCompleteService = {
  get: async (service: string, query: string) => {
    if (service == 'student') {
      const result = await StudentService.search(query);
      const studens = result.data as Student[];
      return studens.map((item) => {
        return { id: item.id, name: item.name, data: item } as AutoCompleteSuggestion
      })
    }

    if (service == 'teacher') {
      const result = await TeacherService.search(query);
      const teachers = result.data as Teacher[];
      return teachers.map((item) => {
        return { id: item.id, name: item.name, data: item } as AutoCompleteSuggestion
      })
    }

  }
}

export default AutoCompleteService;
