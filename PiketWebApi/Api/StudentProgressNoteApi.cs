using ErrorOr;
using Microsoft.EntityFrameworkCore;
using PiketWebApi.Abstractions;
using PiketWebApi.Data;
using PiketWebApi.Services;
using SharedModel.Models;
using SharedModel.Requests;

namespace PiketWebApi.Api
{
    public static class StudentProgressNoteApi
    {
        public static RouteGroupBuilder MapStudentProgressNoteApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllStudentProgressNote);
            group.MapGet("/{id}", GetStudentProgressNoteById);
            group.MapGet("/bystudentid/{id}", GetStudentProgressNoteByStudentId);
            group.MapPost("/", PostStudentProgressNote);
            group.MapPut("/{id}", PutStudentProgressNote);
            group.MapDelete("/{id}", DeleteStudentProgressNote);
            return group.WithTags("studentprogressnote").RequireAuthorization(); ;
        }

        private static async Task<IResult> GetStudentProgressNoteById(HttpContext context, IStudentProgressNoteService studentNoteService, int id)
        {

            var result = await studentNoteService.GetByIdAsync(id);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> GetStudentProgressNoteByStudentId(HttpContext context, IStudentProgressNoteService studentNoteService, int id)
        {

            var result = await studentNoteService.GetByStudentIdAsync(id);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> DeleteStudentProgressNote(HttpContext context, IStudentProgressNoteService studentNoteService, int id)
        {
            var result = await studentNoteService.DeleteAsync(id);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));

        }

        private static async Task<IResult> PutStudentProgressNote(HttpContext context, IStudentProgressNoteService studentNoteService, int id, StudentProgressNoteRequest model)
        {
            var result = await studentNoteService.PutAsync(id, model);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> PostStudentProgressNote(HttpContext context, IStudentProgressNoteService studentNoteService, StudentProgressNoteRequest model)
        {
            var result = await studentNoteService.PostAsync(model);
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }

        private static async Task<IResult> GetAllStudentProgressNote(HttpContext context, IStudentProgressNoteService studentNoteService)
        {
            var result = await studentNoteService.GetAsync();
            return result.Match(items => Results.Ok(items), errors => Results.BadRequest(result.CreateProblemDetail(context)));
        }
    }
}
