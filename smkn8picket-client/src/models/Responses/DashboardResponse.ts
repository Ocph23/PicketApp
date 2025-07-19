import type StudentAttendanceRequest from "../Requests/StudentAttendanceRequest"

export default interface DashboardResponse {
  schoolYearId: number
  schoolYear: number
  schoolYearName: string
  semester: number
  semesterName: string
  totalStudents: number
  totalTeachers: number
  totalClassrooms: number
  totalMaleStudents: number
  totalFemaleStudents: number
  totalMaleTeachers: number
  totalFemaleTeachers: number
  totalDepartments: number
  kehadirans: Kehadiran[]
}


interface Kehadiran {
  groupName: string
  data: StudentAttendanceRequest[]
}
