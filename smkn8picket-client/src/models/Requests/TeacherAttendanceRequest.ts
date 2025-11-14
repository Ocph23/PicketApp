import type { DateTime } from 'luxon'

export default interface TeacherAttendanceRequest {
  id: string
  picketId: number | undefined
  picketDate: string | number | Date | DateTime
  studentId: number
  schoolYearId: number
  status: number
  timeIn: string
  timeOut: string
  description: string
  monthYear: string
}
