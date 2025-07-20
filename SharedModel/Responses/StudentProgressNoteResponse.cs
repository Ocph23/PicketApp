using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Responses
{
    public record StudentProgressNoteResponse(
        int Id,
        int SchoolYearId,
        string? SchoolYearName,
        int StudentId,
        string? StudentName,
        int TeacherId,
        string? TeacherName,
        StudentProgressNoteType ProgressType,
        string Note,
        DateTime CreatedAt,
        DateTime? UpdatedAt
        );


}
