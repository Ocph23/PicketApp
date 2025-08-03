using ModelContextProtocol.Server;
using PiketWebApi.Services;
using System.ComponentModel;
using System.Text.Json;

namespace PiketWebApi.Mcp
{

    [McpServerResourceType]
    public class StudentResources
    {
        private readonly IStudentService studentService;

        public StudentResources(IStudentService _studentService)
        {
            studentService = _studentService;
        }


        [McpServerResource, Description("Get all students")]
        public async Task<string> GetStudents()
        {
            var data = await studentService.GetAllStudent();
            return JsonSerializer.Serialize(data.Value);   
        }
    }
}
