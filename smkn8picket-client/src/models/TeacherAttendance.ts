export default interface TeacherAttendance {
  id: string
  picketId: number
  picketDate: Date
  studentId: number
  studentName: string
  className: string
  departmentName: string
  status: number
  timeIn: string
  timeOut: string
  description: string
  createAt: string
  schoolYearId: number
  monthYear: string
}
