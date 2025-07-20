using SharedModel;

namespace PiketWebApi.Data
{
    public class StudentProgressNote
    {
        public int Id { get; set; }
        public StudentProgressNoteType ProgressType { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public string? Note { get; set; }
        public int SchoolYearId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
