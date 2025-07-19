export default interface LateAndComeHomeEarlyRequest {
  StudentId: number
  AtTime: string
  Description: string
  AttendanceStatus: number
  LateAndGoHomeEarlyStatus: number
}

export interface LateAndComeHomeEarlyResponse {
  id: number
  studentId: number
  studentName?: string
  studentPhoto?: string
  attendanceStatus: number
  lateAndGoHomeEarlyStatus: number
  description?: string
  teacherId: number
  teacherName?: string
  teacherPhoto?: string
  createAt: Date;
  time: Date;
  classRoomId?: number
  classRoomName?: string
  departmentId: number
  departmentName?: string
  studentInitial: string
  teacherInitial: string

}
