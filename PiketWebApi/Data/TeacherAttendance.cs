using SharedModel;
using SharedModel.Models;

namespace PiketWebApi.Data
{
   
    public class TeacherAttendance :Attendance
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
