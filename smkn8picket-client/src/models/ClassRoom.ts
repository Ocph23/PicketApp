import type Student from "./Student"

export default interface ClassRoom {
  id: number
  className: string
  level: number
  schoolYearId: number
  year: number
  departmentId: number
  departmentName: string
  departmentInitial: string
  classLeaderId: number
  classLeaderName: string
  homeRoomTeacherId: number
  homeRoomTeacherName: string
  students: Student[]
}
