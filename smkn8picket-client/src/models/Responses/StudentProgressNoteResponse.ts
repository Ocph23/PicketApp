import type { StudentProgressNoteType } from "../Enums/StudentProgressNoteType";

export default interface StudentProgressNoteResponse {
  id: number;
  schoolYearId: number;
  schoolYearName?: string
  studentId: number;
  studentName?: string
  teacherId: number
  teacherName?: string
  progressType: StudentProgressNoteType
  note: string;
  createdAt: Date
  updatedAt?: Date
}
