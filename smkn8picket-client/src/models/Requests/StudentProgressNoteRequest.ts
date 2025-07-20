import type { StudentProgressNoteType } from "../Enums/StudentProgressNoteType"

export default interface StudentProgressNoteRequest {
  id: number
  studentId: number
  teacherId: number
  note: string
  createdAt: Date | string
  updatedAt?: Date
  progressType: StudentProgressNoteType
}
