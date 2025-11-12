
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PiketWebApi.Abstractions;
using PiketWebApi.Data;
using PiketWebApi.Services;
using SharedModel.Models;
using SharedModel.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PiketWebApi.Api
{
    public static class TeacherAttendanceApi
    {
        public static RouteGroupBuilder MapTeacherAttendanceApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", Get);
            group.MapGet("/{id}", GetById);
            group.MapGet("/byteacherid/{schoolYearId}/{teacherId}", GetAttendanceByTeacherId);
            group.MapGet("/{teacherId}/{month}/{year}", GetByClassRoomAndMonthYear);
            group.MapPost("/", Post);
            group.MapPut("/{id}", Put);
            group.MapDelete("/{id}", Delete);
            group.MapPost("/sync", SyncData);
            return group.WithTags("teacherattendance").RequireAuthorization(); ;
        }

        private static async Task<IResult> GetAttendanceByTeacherId(HttpContext context, ITeacherAttendaceService teacherService, int schoolYearId, int teacherId)
        {
            var result = await teacherService.GetAttendanceByTeacherId(schoolYearId, teacherId);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));

        }

        private static async Task<IResult> GetByClassRoomAndMonthYear(HttpContext context, ITeacherAttendaceService teacherService, int teacherId, int month, int year)
        {
            var result = await teacherService.GetAbsenByTeacherIdMonthYear(teacherId, month,year);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> SyncData(HttpContext context, ITeacherAttendaceService teacherService, List<TeacherAttendanceSyncRequest> data)
        {
            var result = await teacherService.SyncData(data);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }
        private static async Task<IResult> Delete(HttpContext context, ITeacherAttendaceService teacherService, Guid id)
        {
            var result = await teacherService.DeleteAsync(id);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }
        private static async Task<IResult> Post(HttpContext context, ITeacherAttendaceService teacherService, TeacherAttendenceRequest req)
        {
            var result = await teacherService.PostAsync(req);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> Put(HttpContext context, ITeacherAttendaceService teacherService, Guid id, TeacherAttendenceRequest req)
        {
            var result = await teacherService.PutAsync(id, req);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> Get(HttpContext context, ITeacherAttendaceService teacherService)
        {
            var result = await teacherService.GetAsync();
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> GetById(HttpContext context, ITeacherAttendaceService teacherService, Guid id)
        {
            var result = await teacherService.GetByIdAsync(id);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }
    }
}
