using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using PiketWebApi.Services;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PiketWebApi.Mcp.Tools
{

    [McpServerToolType]
    public class StudentAttendanceTools
    {
        private readonly IStudentAttendaceService studentAttendanceService;
        private readonly ISchoolYearService schooleYearService;

        public StudentAttendanceTools(IStudentAttendaceService _studentAttendanceService, ISchoolYearService _schooleYearService)
        {
            studentAttendanceService = _studentAttendanceService;
            schooleYearService = _schooleYearService;
        }


        [McpServerTool(Name = "GetAllStudentAttendance"), Description("Get All Student Attendance in a Active School Year")]
        public async Task<string> GetAllStudentAttendance()
        {
            var data = await studentAttendanceService.GetAsync();
            return JsonSerializer.Serialize(data.Value, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            });
        }


        [McpServerTool(Name = "GetAllAttendanceByStudentByStudentId"), Description("Get All Attendance of Student by student id in a Active School Year")]
        public async Task<string> GetAllStudentAttendance(int studentId)
        {
            var schoolyearActive = await schooleYearService.GetActiveSchoolYear();
            if (schoolyearActive.IsError)
                return "Tahun Ajaran Aktif Tidak Ditemukan !";

            var data = await studentAttendanceService.GetAttendanceByStudentId(schoolyearActive.Value.Id, studentId);
            return JsonSerializer.Serialize(data.Value);
        }



        [McpServerTool(Name = "GetAllStudentAttendanceToday" ), Description("Get All Student  Attendance Today")]
        public async Task<string> GetAllStudentAttendanceToday(DateOnly date)
        {
            var schoolyearActive = await schooleYearService.GetActiveSchoolYear();
            if (schoolyearActive.IsError)
                return "Tahun Ajaran Aktif Tidak Ditemukan !";

            var data = await studentAttendanceService.GetAsync();
            return JsonSerializer.Serialize(data.Value.Where(x=> DateOnly.FromDateTime(x.TimeIn.Value) ==date));
        }

    }
}
