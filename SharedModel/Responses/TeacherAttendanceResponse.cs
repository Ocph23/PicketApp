using System.Text;

namespace SharedModel.Responses;

public record TeacherAttendanceResponse(
       Guid Id,
       int PicketId,
       int TeacherId,
       string TeacherName,
       AttendanceStatus Status,
       DateTime? TimeIn,
       DateTime? TimeOut,
       string? Description,
       DateTime? CreateAt);
