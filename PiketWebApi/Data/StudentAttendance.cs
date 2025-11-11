using SharedModel;

namespace PiketWebApi.Data
{
    public class StudentAttendance:Attendance
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
