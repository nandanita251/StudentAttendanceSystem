using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Data;
using StudentAttendanceSystem.Models;
using StudentAttendanceSystem.ViewModels;

namespace StudentAttendanceSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> MarkAttendanceAsync(MarkAttendanceViewModel model)
        {
            try
            {
                var existingAttendance = await _context.Attendances
                    .Where(a => a.SubjectId == model.SubjectId && a.Date.Date == model.Date.Date)
                    .ToListAsync();

                if (existingAttendance.Any())
                {
                    _context.Attendances.RemoveRange(existingAttendance);
                }

                foreach (var student in model.Students)
                {
                    var attendance = new Attendance
                    {
                        StudentId = student.StudentId,
                        SubjectId = model.SubjectId,
                        Date = model.Date.Date,
                        Status = student.IsPresent,
                        Remarks = student.Remarks,
                        CreatedAt = DateTime.Now
                    };
                    _context.Attendances.Add(attendance);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Attendance>> GetAttendanceByDateAsync(int subjectId, DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .Where(a => a.SubjectId == subjectId && a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<bool> IsAttendanceMarkedAsync(int subjectId, DateTime date)
        {
            return await _context.Attendances
                .AnyAsync(a => a.SubjectId == subjectId && a.Date.Date == date.Date);
        }

        public async Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return await _context.Students
                .Where(s => s.Class == className)
                .OrderBy(s => s.RollNo)
                .ToListAsync();
        }
    }
}
