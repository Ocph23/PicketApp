using FluentValidation;
using FluentValidation.Validators;
using PiketWebApi.Data;

namespace PiketWebApi.Validators
{
    internal class TeacherAttendanceValidator : AbstractValidator<TeacherAttendance>
    {
        public TeacherAttendanceValidator()
        {
            RuleFor(x => x.Teacher).NotEmpty().WithMessage("Guru tidak boleh kosong");
            RuleFor(x => x.AttendanceStatus).NotEmpty().WithMessage("Status Kehadiran tidak boleh kosong");
            RuleFor(x => x.TimeIn).NotEmpty().WithMessage("Jam Masuk");
        }
    }
}