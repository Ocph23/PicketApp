import type StudentAttendance from "./StudentAttendance";
import type Teacher from "./Teacher";
import type DailyJournal from "./DailyJournal";
import type { LateAndComeHomeEarlyResponse } from "./LateAndComeHomeEarly";

export default interface Picket {
  id: number;
  date: string;
  weather: number;
  startAt: string;
  endAt: string;
  createdId: number;
  createdName: string;
  createdNumber: string;
  createAt: string;
  teacherAttendance?: Teacher[];
  studentsLateAndComeHomeEarly?: LateAndComeHomeEarlyResponse[];
  studentAttendance?: StudentAttendance[];
  dailyJournal?: DailyJournal[]

}
