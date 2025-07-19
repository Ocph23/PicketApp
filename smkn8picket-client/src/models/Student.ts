export default interface Student {
  nis: string
  nisn: string
  id: number
  gender: number
  name: string
  placeOfBorn: string
  dateOfBorn: string | null
  email: string
  description: string
  photo: string
  userId: string
  parentPhoneNumber: string
  status: number
}

export interface ClassRoom {
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
