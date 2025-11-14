using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModel;
using System.Text.Json;

namespace PiketWebApi.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceScope scope)
        {
            var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbcontext.Database.MigrateAsync(); // Apply pending migrations

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            if (!dbcontext.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                await roleManager.CreateAsync(new IdentityRole { Name = "HeadOfSchool" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Teacher" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Student" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Staff" });
            }

            if (!dbcontext.Users.Any())
            {
                var user = new ApplicationUser("admin@picket.smkn8tikjayapura.sch.id") { Name = "Admin", Email = "admin@picket.smkn8tikjayapura.sch.id", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, "Password@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await userManager.DeleteAsync(user);
                }
            }

            if (!dbcontext.Departments.Any())
            {
                dbcontext.Departments.AddRange(new List<Department>
                {
                    new Department { Name = "Design Komunikas dan Visual" , Initial="DKV", Description=string.Empty },
                    new Department { Name = "Teknik Komunikasi dan Jaringan" , Initial="TKJ", Description=string.Empty },
                    new Department { Name = "Rekayasa Perangkat Lunak" , Initial="RPL", Description=string.Empty },
                    new Department { Name = "Kimia Industri" , Initial="KI", Description=string.Empty },
                });
                dbcontext.SaveChanges();
            }


            if (!dbcontext.Teachers.Any())
            {
                CancellationToken ct = default;
                var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "teacher.json");
                if (!File.Exists(jsonFilePath)) throw new FileNotFoundException(jsonFilePath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                await using var fs = File.OpenRead(jsonFilePath);
                var dtos = await JsonSerializer.DeserializeAsync<List<Teacher>>(fs, options, ct)
                           ?? new List<Teacher>();


                foreach (var teacher in dtos)
                {
                    var user = new ApplicationUser(teacher.Email) { Name = teacher.Name, Email = teacher.Email, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Teacher");
                        teacher.UserId = user.Id;
                        await dbcontext.Teachers.AddAsync(teacher);
                    }
                    else
                    {
                        await userManager.DeleteAsync(user);
                    }
                }
            }


            if(!dbcontext.Students.Any())
            {
                CancellationToken ct = default;
                var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "student.json");
                if (!File.Exists(jsonFilePath)) throw new FileNotFoundException(jsonFilePath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                await using var fs = File.OpenRead(jsonFilePath);
                var dtos = await JsonSerializer.DeserializeAsync<List<Student>>(fs, options, ct)
                           ?? new List<Student>();
                dbcontext.Students.AddRange(dtos);
                await dbcontext.SaveChangesAsync(ct);

            }

        }
    }

}
