import type { AbsenPrintModel, Student } from '@/models'
import { defineStore } from 'pinia'

const PrintStore = defineStore('print', {
  // arrow function recommended for full type inference
  state: () => {
    return {
      // all these properties will have their type inferred automatically
      studentClassMemberPrint: {} as Student[],
      dataSiswa: [] as Array<AbsenPrintModel>,
      dataTanggal: [] as Array<string>
    }
  },
  actions: {
    setStudentClassMember(students: Student[]) {
      this.studentClassMemberPrint = students;
    },
    setAbsen(dataSiswax: Array<AbsenPrintModel>, dataTanggalx: Array<string>) {
      this.dataSiswa = dataSiswax;
      this.dataTanggal = dataTanggalx;
    }
  },
})
export default PrintStore
