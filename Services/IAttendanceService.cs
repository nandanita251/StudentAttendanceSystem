using StudentAttendanceSystem.Models;
using StudentAttendanceSystem.ViewModels;

namespace StudentAttendanceSystem.Services
{
    public interface IAttendanceService
    {
        Task<bool> MarkAttendanceAsync(MarkAttendanceViewModel model);
        Task<List<Attendance>> GetAttendanceByDateAsync(int subjectId, DateTime date);
        Task<bool> IsAttendanceMarkedAsync(int subjectId, DateTime date);
        Task<List<Student>> GetStudentsByClassAsync(string className);
    }
}
