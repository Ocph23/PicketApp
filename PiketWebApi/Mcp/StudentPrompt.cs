using ModelContextProtocol.Server;
using PiketWebApi.Services;
using System.ComponentModel;
using System.Text.Json;

namespace PiketWebApi.Mcp
{

    [McpServerPromptType]
    public class StudentPrompt
    {
        private readonly IStudentService studentService;

        public StudentPrompt(IStudentService _studentService)
        {
            studentService = _studentService;
        }


        [McpServerPrompt, Description("Get all students")]
        public async Task<string> GetStudents()
        {
            var data = await studentService.GetAllStudent();
            return JsonSerializer.Serialize(data.Value);   
        }


        [McpServerPrompt, Description("Get student")]
        public async Task<string> GetStudent(int id)
        {
            var data = await studentService.GetStudentById(id);
            return JsonSerializer.Serialize(data.Value);
        }

    }
}
