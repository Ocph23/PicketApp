using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PiketWebApi.Data;
using PiketWebApi.Services;
using SharedModel;
using SharedModel.Models;
using SharedModel.Requests;
using SharedModel.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PiketWebApi.Api
{
    public static class AuthApi
    {

        public static RouteGroupBuilder MapAuthApi(this RouteGroupBuilder group)
        {
            group.MapPost("/login", LoginAction).AllowAnonymous();
            group.MapPost("/register", RegisterAction).AllowAnonymous();
            group.MapGet("/lockuser/{userId}", LockAccout);
            group.MapGet("/unlockuser/{userId}", UnLockAccout);
            group.MapGet("/status/{userId}", ActiveAccout);
            group.MapGet("/isadmin/{userId}", IsAdmin);
            group.MapGet("/setadmin/{userId}", SetAsAdmin);
            group.MapGet("/resetpassword/{email}", ResetPasswordByAdmin);
            group.MapGet("/removeasadmin/{userId}", RemoveAsAdmin);
            group.MapGet("/resetpassword", ResetPassword);
            group.MapPost("/changepassword", ChangePassword);
            return group.RequireAuthorization("administrator");
        }
        private static async Task<IResult> ChangePassword(HttpContext context, UserManager<ApplicationUser> userManager, ChangePasswordRequest model)
        {
            try
            {
                if (await userManager.FindByEmailAsync(model.UserName) is ApplicationUser user)
                {
                    await userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
                    return Results.Ok();
                }

                ErrorOr<bool> error = Error.Failure("Data user tidak ditemukan");
                return Results.BadRequest(error.CreateProblemDetail(context));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> ResetPasswordByAdmin(HttpContext context, UserManager<ApplicationUser> userManager, string email)
        {
            try
            {
                if (email != null && await userManager.FindByEmailAsync(email) is ApplicationUser user)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, token, "Password@123");
                    return Results.Ok();
                }

                ErrorOr<bool> error = Error.Failure("Data user tidak ditemukan");
                return Results.BadRequest(error.CreateProblemDetail(context));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> ResetPassword(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var userId = context.User.Claims.FirstOrDefault()?.Value;

                if (userId != null && await userManager.FindByIdAsync(userId) is ApplicationUser user)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    return Results.Ok(token);
                }

                ErrorOr<bool> error = Error.Failure("Data user tidak ditemukan");
                return Results.BadRequest(error.CreateProblemDetail(context));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> RemoveAsAdmin(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.RemoveFromRoleAsync(user, "Admin");
                    return Results.Ok();
                }

                ErrorOr<bool> error = Error.Failure("Data user tidak ditemukan");
                return Results.BadRequest(error.CreateProblemDetail(context));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> SetAsAdmin(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    return Results.Ok();
                }

                ErrorOr<bool> error = Error.Failure("Data user tidak ditemukan");
                return Results.BadRequest(error.CreateProblemDetail(context));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> ActiveAccout(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new SystemException();

                var IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
                var Roles = await userManager.GetRolesAsync(user);

                var obj = new { Activated = !user.LockoutEnabled, IsAdmin, Roles };
                return TypedResults.Ok(obj);
            }
            catch (Exception)
            {
                return TypedResults.Unauthorized();
            }
        }
        private static async Task<IResult> LockAccout(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new SystemException();

                var result = await userManager.SetLockoutEnabledAsync(user, true);
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(100).ToUniversalTime());

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.Unauthorized();
            }
        }

        private static async Task<IResult> UnLockAccout(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new SystemException();

                var result = await userManager.SetLockoutEnabledAsync(user, false);

                return TypedResults.Ok(result);
            }
            catch (Exception)
            {
                return TypedResults.Unauthorized();
            }
        }

        private static IResult IsAdmin(HttpContext context)
        {
            try
            {
                return TypedResults.Ok();
            }
            catch (Exception)
            {
                return TypedResults.Unauthorized();
            }
        }

        private static async Task RegisterAction(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task<IResult> LoginAction(HttpContext context,
           SharedModel.Requests.LoginRequest request,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration _config,
            ApplicationDbContext dbContext
            )
        {
            try
            {
                var students = dbContext.Students.ToList();
                AppSettings _appSettings = new AppSettings();
                _config.GetSection("AppSettings").Bind(_appSettings);
                var result = await signInManager.PasswordSignInAsync(request.Username.ToUpper(), request.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(request.Username.ToUpper());
                    ArgumentNullException.ThrowIfNull(user);
                    var identity = user as ApplicationUser;
                    if (!await userManager.IsEmailConfirmedAsync(user))
                        throw new SystemException("Email Belum diverifikasi , Silahkan Hubungi Administrator");
                    var roles = await userManager.GetRolesAsync(user);
                    var token = await user.GenerateToken(_appSettings, roles);
                    Profile? profile = null;
                    if (roles.Contains("Teacher"))
                    {
                        profile = dbContext.Teachers.FirstOrDefault(x => x.UserId == identity.Id);
                        var classrooms = from t in dbContext.Teachers.Where(x => x.UserId == user.Id)
                                         join c in dbContext.ClassRooms.Include(x => x.HomeroomTeacher) on t.Id equals c.HomeroomTeacher.Id
                                         select c;

                        if (classrooms.Count() > 0)
                        {
                            roles.Add("HomeRoomTeacher");
                        }

                    }

                    if (roles.Contains("Student"))
                    {
                        profile = dbContext.Students.FirstOrDefault(x => x.UserId == identity.Id);
                    }


                    return Results.Ok(new AuthenticateResponse
                    (user.Name, user.Email, roles, token, profile));
                }


                if (result.IsNotAllowed || result.IsLockedOut)
                    throw new SystemException("Akun Anda Di Blokir , Silahkan Hubungi Administrator");
                else
                    throw new SystemException($"Masukkan user dan password dengan benar !");
            }
            catch (System.Exception ex)
            {
                return Results.Json(Helper.CreateBadRequestProbleDetail(context, ex), statusCode: StatusCodes.Status401Unauthorized);
            }
        }
    }


    public static class AuthServiceExtention
    {
        public static Task<string> GenerateToken<ApplicationUser>(this ApplicationUser tuser, AppSettings _appSettings, IList<string>? roles = null)
        {

            var user = tuser as IdentityUser;
            var issuer = _appSettings.Issuer;
            var audience = _appSettings.Audience;
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            IList<Claim>? claims = new List<Claim>() {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };


            foreach (var item in roles)
            {
                claims.Add(new Claim("role", item));
            }

            var token = new JwtSecurityToken(issuer: issuer, audience, claims, expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature));

            var tokenHandler = new JwtSecurityTokenHandler();
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

    }
}
