

using SharedModel;
using SharedModel.Responses;

namespace PiketWebApi.Data
{
    public class Teacher : Profile
    {
        public string? RegisterNumber { get; set; }
        public Job Job { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
