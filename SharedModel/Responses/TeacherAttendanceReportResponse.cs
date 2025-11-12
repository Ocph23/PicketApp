using System;

namespace SharedModel.Responses;
public class TeacherAttendanceReportResponse
{
    public int? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public string? ClassRoomName { get; set; }
    public string? DepartmentName { get; set; }
    public int PicketId { get; set; }
    public DateOnly PicketDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public object TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    public string? Description { get; set; }
    public int SchoolYearId { get; set; }
}