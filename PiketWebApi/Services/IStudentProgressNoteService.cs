using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PiketWebApi.Data;
using SharedModel.Requests;
using SharedModel.Responses;
using System.Linq;

namespace PiketWebApi.Services
{
    public interface IStudentProgressNoteService
    {
        Task<ErrorOr<bool>> DeleteAsync(int id);
        Task<ErrorOr<bool>> PutAsync(int id, StudentProgressNoteRequest model);
        Task<ErrorOr<StudentProgressNoteResponse>> PostAsync(StudentProgressNoteRequest model);
        Task<ErrorOr<IEnumerable<StudentProgressNoteResponse>>> GetAsync();
        Task<ErrorOr<StudentProgressNoteResponse>> GetByIdAsync(int id);
        Task<ErrorOr<IEnumerable<StudentProgressNoteResponse>>> GetByStudentIdAsync(int studentId);
    }

    public class StudentProgressNoteService : IStudentProgressNoteService
    {
        private readonly IHttpContextAccessor http;
        private readonly ISchoolYearService schoolYearService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public StudentProgressNoteService(
            IHttpContextAccessor _http,
            ISchoolYearService _schoolYearService,
            UserManager<ApplicationUser> _userManager,
            ApplicationDbContext _dbContext)
        {
            http = _http;
            schoolYearService = _schoolYearService;
            userManager = _userManager;
            dbContext = _dbContext;
        }


        public async Task<ErrorOr<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = dbContext.Teachers.SingleOrDefault(x => x.Id == id);
                if (result == null)
                    return Error.NotFound("Teacher", "Data guru tidak ditemukan.");


                dbContext.Remove(result);
                dbContext.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<bool>> PutAsync(int id, StudentProgressNoteRequest req)
        {
            try
            {
                var schoolYearResult = await schoolYearService.GetActiveSchoolYear();
                if (schoolYearResult.IsError)
                    return Error.NotFound("Tahun ajaran aktif tidak ditemukan/belum ada");

                if(schoolYearResult.Value.Id != req.SchoolYearId)
                    return Error.Conflict("Tahun ajaran progress ini sudah tidak aktif, data tidak dapat diubah");

                var result = dbContext.StudentProgressNotes.SingleOrDefault(x => x.Id == id);
                if (result == null)
                {
                    return Error.NotFound("Data guru tidak ditemukan.");
                }

                result.ProgressType = req.ProgressType;
                result.Note = req.Note;
                result.UpdatedAt = DateTime.Now.ToUniversalTime();
                var validator = new Validators.StudentProgressNoteValidator();
                var validatorResult = validator.Validate(result);
                if (!validatorResult.IsValid)
                    return validatorResult.GetErrors();
                dbContext.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<StudentProgressNoteResponse>> PostAsync(StudentProgressNoteRequest req)
        {
            try
            {
                var schoolYearResult = await schoolYearService.GetActiveSchoolYear();
                if (schoolYearResult.IsError)
                    return Error.NotFound("Tahun ajaran aktif tidak ditemukan/belum ada");

                var model = new StudentProgressNote
                {
                    Note = req.Note,
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    SchoolYearId = schoolYearResult.Value.Id,
                    TeacherId = req.TeacherId,
                    StudentId = req.StudentId,
                    ProgressType = req.ProgressType
                };

                var validator = new Validators.StudentProgressNoteValidator();
                var validatorResult = validator.Validate(model);
                if (!validatorResult.IsValid)
                    return validatorResult.GetErrors();


                var result = dbContext.StudentProgressNotes.Add(model);
                dbContext.SaveChanges();
                var response = await GetByIdAsync(model.Id);
                return response.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ErrorOr<IEnumerable<StudentProgressNoteResponse>>> GetAsync()
        {
            try
            {
                var result = from p in dbContext.StudentProgressNotes
                             join sy in dbContext.SchoolYears on p.SchoolYearId equals sy.Id
                             join st in dbContext.Students on p.StudentId equals st.Id
                             join tc in dbContext.Teachers on p.TeacherId equals tc.Id
                             select new StudentProgressNoteResponse(
                                 p.Id, p.SchoolYearId, $"{sy.Name} {sy.SemesterName}",
                                 p.StudentId, st.Name, p.TeacherId, tc.Name, p.ProgressType, p.Note, p.CreatedAt, p.UpdatedAt
                                 );
                return result.ToList();
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<StudentProgressNoteResponse>> GetByIdAsync(int id)
        {
            try
            {
                var result = from p in dbContext.StudentProgressNotes
                             where p.Id == id
                             join sy in dbContext.SchoolYears on p.SchoolYearId equals sy.Id
                             join st in dbContext.Students on p.StudentId equals st.Id
                             join tc in dbContext.Teachers on p.TeacherId equals tc.Id
                             select new StudentProgressNoteResponse(
                                 p.Id, p.SchoolYearId, $"{sy.Name} {sy.SemesterName}",
                                 p.StudentId, st.Name, p.TeacherId, tc.Name, p.ProgressType, p.Note, p.CreatedAt, p.UpdatedAt
                                 );
                if (!result.Any())
                    return Error.NotFound("NotFound", "Data guru tidak ditemukan");
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<IEnumerable<StudentProgressNoteResponse>>> GetByStudentIdAsync(int id)
        {
            try
            {
                var result = from p in dbContext.StudentProgressNotes
                             where p.StudentId == id
                             join sy in dbContext.SchoolYears on p.SchoolYearId equals sy.Id
                             join st in dbContext.Students on p.StudentId equals st.Id
                             join tc in dbContext.Teachers on p.TeacherId equals tc.Id
                             select new StudentProgressNoteResponse(
                                 p.Id, p.SchoolYearId, $"{sy.Name} {sy.SemesterName}",
                                 p.StudentId, st.Name, p.TeacherId, tc.Name, p.ProgressType, p.Note, p.CreatedAt, p.UpdatedAt
                                 );
                return result.ToList();
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }
    }
}
