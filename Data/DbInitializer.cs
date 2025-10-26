using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Models;

namespace StudentAttendanceSystem.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            string[] roleNames = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@attendance.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            var teacherEmail = "teacher@attendance.com";
            var teacherUser = await userManager.FindByEmailAsync(teacherEmail);
            
            if (teacherUser == null)
            {
                var teacher = new User
                {
                    UserName = teacherEmail,
                    Email = teacherEmail,
                    EmailConfirmed = true,
                    FullName = "John Doe"
                };

                var result = await userManager.CreateAsync(teacher, "Teacher@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(teacher, "Teacher");
                    
                    var teacherRecord = new Teacher
                    {
                        Name = "John Doe",
                        Email = teacherEmail,
                        Subject = "Computer Science",
                        UserId = teacher.Id
                    };
                    context.Teachers.Add(teacherRecord);
                    await context.SaveChangesAsync();
                }
            }

            var studentEmail = "student@attendance.com";
            var studentUser = await userManager.FindByEmailAsync(studentEmail);
            
            if (studentUser == null)
            {
                var student = new User
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true,
                    FullName = "Jane Smith"
                };

                var result = await userManager.CreateAsync(student, "Student@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Student");
                    
                    var studentRecord = new Student
                    {
                        Name = "Jane Smith",
                        Email = studentEmail,
                        Class = "CS-3A",
                        RollNo = "CS2023001",
                        UserId = student.Id
                    };
                    context.Students.Add(studentRecord);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
