export default interface StudentProgressNoteResponse {
  id: number;
  schoolYearId: number;
  schoolYearName?: string
  studentId: number;
  studentName?: string
  teacherId: number
  teacherName?: string
  note: string;
  createdAt: Date
  updatedAt?: Date
}
