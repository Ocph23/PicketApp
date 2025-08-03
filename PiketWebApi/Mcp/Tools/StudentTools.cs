using ModelContextProtocol.Server;
using PiketWebApi.Services;
using System.ComponentModel;
using System.Text.Json;

namespace PiketWebApi.Mcp.Tools
{

    [McpServerToolType]
    public class StudentTools
    {
        private readonly IStudentService studentService;

        public StudentTools(IStudentService _studentService)
        {
            studentService = _studentService;
        }


        [McpServerTool, Description("Get all students")]
        public async Task<string> GetStudents()
        {
            var data = await studentService.GetAllStudent();
            return JsonSerializer.Serialize(data.Value);
        }


        [McpServerTool, Description("Get student")]
        public async Task<string> GetStudent(int id)
        {
            var data = await studentService.GetStudentById(id);
            return JsonSerializer.Serialize(data.Value);
        }



        [McpServerTool, Description("Ulang Tahun Siswa")]
        public async Task<string> SiswaBerulangtahun (DateOnly date)
        {
            var data = await studentService.GetAllStudent();
            if(!data.IsError)
                return JsonSerializer.Serialize(data.Value.Where(x=>x.DateOfBorn.Month==date.Month));

            return "Data siswa tidak ditemukan !";
        }

    }
}
