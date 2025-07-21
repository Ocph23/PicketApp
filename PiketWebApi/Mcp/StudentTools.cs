using ModelContextProtocol.Server;
using PiketWebApi.Services;
using System.ComponentModel;
using System.Text.Json;

namespace PiketWebApi.Mcp
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

    }
}
