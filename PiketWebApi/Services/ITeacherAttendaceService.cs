using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PiketWebApi.Data;
using PiketWebApi.Validators;
using SharedModel;
using SharedModel.Requests;
using SharedModel.Responses;
using System.Net.Sockets;
using Error = ErrorOr.Error;

namespace PiketWebApi.Services
{
    public interface ITeacherAttendaceService
    {
        Task<ErrorOr<IEnumerable<TeacherAttendanceResponse>>> GetAsync();
        Task<ErrorOr<TeacherAttendanceResponse>> GetByIdAsync(Guid id);
        Task<ErrorOr<bool>> DeleteAsync(Guid id);
        Task<ErrorOr<bool>> PutAsync(Guid id, TeacherAttendenceRequest model);
        Task<ErrorOr<TeacherAttendanceResponse>> PostAsync(TeacherAttendenceRequest model);
        Task<ErrorOr<IEnumerable<TeacherAttendanceSyncRequest>>> SyncData(IEnumerable<TeacherAttendanceSyncRequest> data);
        Task<ErrorOr<IEnumerable<TeacherAttendanceReportResponse>>> GetAttendanceByTeacherId(int yearSchoolId, int teacherId);
        Task<ErrorOr<TeacherAttendanceReportResponse>> GetAttendaceMonthYear(int month, int year);
    }


    public class TeacherAttendaceService : ITeacherAttendaceService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ITeacherService teacherService;
        private readonly IPicketService picketService;
        private readonly IHttpClientFactory factory;
        private readonly HttpClient waAppClient;

        public DateTime CreatedAt { get; private set; }

        public TeacherAttendaceService(
            UserManager<ApplicationUser> _userManager,
            ApplicationDbContext _dbContext,
              ITeacherService _teacherService, IPicketService _picketService, IHttpClientFactory _factory
            )
        {
            userManager = _userManager;
            dbContext = _dbContext;
            teacherService = _teacherService;
            picketService = _picketService;
            factory = _factory;
            this.waAppClient = factory.CreateClient("waapp");
        }

        public async Task<ErrorOr<IEnumerable<TeacherAttendanceResponse>>> GetAsync()
        {
            try
            {
                var teachers = await teacherService.GetAsync();
                if (teachers.IsError)
                    return teachers.Errors;

                var result = from a in dbContext.TeacherAttendances.Include(x => x.Teacher).AsEnumerable()
                             join b in teachers.Value on a.Teacher.Id equals b.Id
                             select new TeacherAttendanceResponse(a.Id, a.PicketId,
                             b.Id, b.Name, a.AttendanceStatus, a.TimeIn, a.TimeOut, a.Description, a.CreateAt);
                return result.ToList();
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<TeacherAttendanceResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var teachers = await teacherService.GetAsync();
                if (teachers.IsError)
                    return teachers.Errors;

                var result = from a in dbContext.TeacherAttendances.Include(x => x.Teacher).Where(x => x.Id == id).ToList()
                             join b in teachers.Value on a.Teacher.Id equals b.Id
                             select new TeacherAttendanceResponse(a.Id, a.PicketId,
                             b.Id, b.Name, a.AttendanceStatus, a.TimeIn, a.TimeOut, a.Description, a.CreateAt);
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = dbContext.TeacherAttendances.SingleOrDefault(x => x.Id == id);
                if (result == null)
                    return Error.NotFound("NotFound", "Data absen tidak ditemukan.");
                dbContext.Remove(result);
                dbContext.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<bool>> PutAsync(Guid id, TeacherAttendenceRequest model)
        {
            try
            {
                var result = dbContext.TeacherAttendances.Include(x => x.Teacher).SingleOrDefault(x => x.Id == id);
                if (result == null)
                {
                    return Error.NotFound("NotFound", "Data Absen Siswa tidak ditemukan.");
                }
                result.TimeIn = model.TimeIn.Value;
                result.TimeOut = model.TimeOut == null ? null : model.TimeOut.Value;
                result.AttendanceStatus = model.Status;
                result.Description = model.Description;
                dbContext.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<TeacherAttendanceResponse>> PostAsync(TeacherAttendenceRequest req)
        {
            try
            {
                var teacher = dbContext.Teachers.SingleOrDefault(x => x.Id == req.TeacherId);
                if (teacher == null)
                    return Error.Conflict("TeacherAttendance", $"Data siswa tidak ditemukan !");

                var now = DateOnly.FromDateTime(DateTime.Now);
                if (dbContext.TeacherAttendances.Any(x => x.Teacher.Id == teacher.Id && DateOnly.FromDateTime(x.TimeIn) == now))
                    return Error.Conflict("Exists TeacherAttendance", $"{teacher.Name} sudah absen !");

                var model = new TeacherAttendance
                {
                    Id = req.Id == Guid.Empty ? Guid.NewGuid() : req.Id,
                    Teacher = teacher,
                    PicketId = req.PicketId,
                    AttendanceStatus = req.Status,
                    TimeIn = req.TimeIn.Value.ToUniversalTime(),
                    TimeOut = req.TimeOut == null ? null : req.TimeOut.Value.ToUniversalTime(),
                    Description = req.Description,
                    CreateAt = DateTime.Now.ToUniversalTime()
                };


                var validator = new TeacherAttendanceValidator();
                ValidationResult validateResult = validator.Validate(model);
                if (!validateResult.IsValid)
                {
                    return validateResult.GetErrors();
                }

                var picket = dbContext.Picket.SingleOrDefault(x => x.Id == req.PicketId);
                //picket.TeacherAttendances.Add(model);
                dbContext.TeacherAttendances.Add(model);
                dbContext.SaveChanges();
                return await GetByIdAsync(model.Id);
            }
            catch (Exception)
            {
                return Error.Conflict();
            }
        }

        public async Task<ErrorOr<IEnumerable<TeacherAttendanceSyncRequest>>> SyncData(IEnumerable<TeacherAttendanceSyncRequest> req)
        {
            try
            {

                var dataToInsert = req.Where(x => x.TimeOut == null).ToList();
                var dataToUpdate = req.Where(x => x.TimeOut != null).ToList();


                foreach (var item in dataToInsert)
                {
                    var model = new TeacherAttendance
                    {
                        Id = item.AbsenId.Value,
                        Teacher = dbContext.Teachers.FirstOrDefault(x => x.Id == item.TeacherId)!,
                        PicketId = item.PicketId,
                        AttendanceStatus = item.Status,
                        TimeIn = item.TimeIn.Value,
                        TimeOut = item.TimeOut,
                        Description = item.Description,
                        CreateAt = DateTime.Now.ToUniversalTime()
                    };

                    dbContext.Entry(model).State = EntityState.Unchanged;
                    dbContext.TeacherAttendances.Add(model);

                }
                dbContext.SaveChanges();

                foreach (var item in dataToUpdate)
                {
                    var x = item.AbsenId.Value;
                    var data = dbContext.TeacherAttendances.Include(x => x.Teacher).FirstOrDefault(x => x.Id == item.AbsenId.Value);
                    dbContext.Entry(data).CurrentValues.SetValues(item);
                }

                dbContext.SaveChanges();

                foreach (var item in req)
                {
                    item.IsSynced = true;
                }
                return req.ToList();
            }
            catch (Exception ex)
            {
                return Error.Conflict();
            }
        }


        public async Task<ErrorOr<IEnumerable<TeacherAttendanceReportResponse>>> GetAttendanceByTeacherId(int yearSchoolId, int teacherId)
        {
            try
            {
                var result = from p in dbContext.Picket.Where(x => x.SchoolYearId == yearSchoolId).Include(x => x.SchoolYear)
                             join a in dbContext.TeacherAttendances.Where(x => x.TeacherId == teacherId) on p.Id equals a.PicketId into xattendate
                             from a in xattendate.DefaultIfEmpty()
                             join s in dbContext.Teachers on a.TeacherId equals s.Id into x
                             from s in x.DefaultIfEmpty()
                             select new TeacherAttendanceReportResponse
                             {
                                 //TeacherId = s == null ? null : s.Id,
                                 //TeacherName = s == null ? null : s.Name,
                                 //PicketId = p.Id,
                                 //PicketDate = p.Date,
                                 //SchoolYearId = p.SchoolYearId,
                                 //Status = a == null ? SharedModel.AttendanceStatus.Alpa : a.AttendanceStatus,
                                 //TimeIn = a == null ? null : a.TimeIn,
                                 //TimeOut = a == null ? null : a.TimeOut,
                                 //Description = a == null ? null : a.Description
                             };




                return await Task.FromResult(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ErrorOr<TeacherAttendanceReportResponse>> GetAttendaceMonthYear(int month, int year)
        {
            try
            {
                var startDate = DateOnly.FromDateTime(new DateTime(year, month, 1));
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var pickets = dbContext.Picket.Where(x => x.Date >= startDate && x.Date <= endDate)
                .Include(x => x.TeacherAttendances).ThenInclude(x => x.Teacher).AsEnumerable();

                var teachers = dbContext.Teachers.AsEnumerable();
                var report = teachers.Select(t => new TeacherAttendanceReport
                {
                    TeacherId = t.Id,
                    TeacherName = t.Name,
                    JobStatus = t.JobStatus,
                    Items = pickets.Select(p =>
                    {
                        var attendance = p.TeacherAttendances
                            .FirstOrDefault(a => a.TeacherId == t.Id);

                        return new TeacherAttendaceReportItem
                        {
                            PicketDate = p.Date,
                            Description = attendance?.Description,
                            PicketId = p.Id,
                            SchoolYearId = p.SchoolYearId,
                            Status = attendance != null ? attendance.AttendanceStatus : AttendanceStatus.Alpa,
                            TimeIn = attendance?.TimeIn,
                            TimeOut = attendance?.TimeOut
                        };
                    })
                });

                TeacherAttendanceReportResponse attendanceReportResponse = new TeacherAttendanceReportResponse();
                attendanceReportResponse.Month = month;
                attendanceReportResponse.Year = year;
                attendanceReportResponse.Pickets = pickets.Select(x => new { x.Id, x.SchoolYear, x.Date, x.EndAt });
                attendanceReportResponse.Attendances = report.ToList();

                return attendanceReportResponse;
            }
            catch (Exception ex)
            {
                return Error.Conflict();
            }
        }
    }
}
