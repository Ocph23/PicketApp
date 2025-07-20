using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.Requests
{
    public record StudentProgressNoteRequest(
        int StudentId,
        int TeacherId,
        int SchoolYearId,
        string Note,
          StudentProgressNoteType ProgressType
        );

}
