using FluentValidation;
using PiketWebApi.Data;

namespace PiketWebApi.Validators
{
    internal class StudentProgressNoteValidator : AbstractValidator<StudentProgressNote>
    {
        public StudentProgressNoteValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("Siswa tidak boleh kosong");
            RuleFor(x => x.TeacherId).GreaterThan(0).WithMessage("Guru tidak boleh kosong");
            RuleFor(x => x.SchoolYearId).GreaterThan(0).WithMessage("Tahun Ajaran tidak boleh kosong");
            RuleFor(x => x.Note).NotEmpty().WithMessage("Catatan tidak boleh kosong");
            RuleFor(x => x.CreatedAt).NotEmpty().WithMessage("Tanggal tidak boleh kosong");
        }
    }
}