export default interface TeacherAttendanceReport {
  month: number
  year: number
  pickets: {
    id: null
    schoolYearId: number
    date: Date
    endAt: string
  }[]
  attendances: {
    teacherId: number
    teacherName: string
    jobStatus : number
    items: {
      picketId: number
      status: number
      timeIn: Date
      timeOut: Date
      description: string
      schoolYearId: number
    }[]
  }[]
}
