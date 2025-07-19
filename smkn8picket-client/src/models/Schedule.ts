export default interface Schedule {
  id: number
  schoolYearId: number
  year: number
  semester: number
  dayOfWeek: string
  teacherId: number
  teacherNumber: string
  teacherName: string
  photo: null | string
  teacherInitial: string
}
