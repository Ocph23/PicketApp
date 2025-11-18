using System;

namespace SharedModel.Responses;


public class TeacherAttendanceReportResponse
{
    public int Month { get; set; }
    public int Year { get; set; }
    public object Pickets { get; set; }
    public List<TeacherAttendanceReport> Attendances { get; set; } = new List<TeacherAttendanceReport>();


}



public class TeacherAttendanceReport
{
    public int? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public JobStatus JobStatus {get;set;}
    public IEnumerable<TeacherAttendaceReportItem> Items { get; set; } = new List<TeacherAttendaceReportItem>();

   
}

public class TeacherAttendaceReportItem
{
    public int PicketId { get; set; }
    public DateOnly PicketDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public object TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    public string? Description { get; set; }
    public int SchoolYearId { get; set; }
}


