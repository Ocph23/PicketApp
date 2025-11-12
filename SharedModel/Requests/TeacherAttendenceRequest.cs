using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Requests
{
    public record TeacherAttendenceRequest(Guid Id,int PicketId, int TeacherId, AttendanceStatus Status, DateTime? TimeIn, DateTime? TimeOut,  string? Description);
}
