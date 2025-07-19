import type Teacher from "./Teacher"

export default interface AuthResponse {
  userName: string
  email: string
  roles: string[]
  token: string
  profile: Teacher
  isAdmin: boolean
  isTeacherPicket: boolean
  isHomeRoomTeacher: boolean
  loginAs: string
}
