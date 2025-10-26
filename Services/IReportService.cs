using StudentAttendanceSystem.ViewModels;

namespace StudentAttendanceSystem.Services
{
    public interface IReportService
    {
        Task<List<AttendanceReportViewModel>> GetClassAttendanceReportAsync(string className, int? subjectId = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<StudentAttendanceDetailViewModel> GetStudentAttendanceDetailAsync(int studentId, DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> ExportAttendanceToExcelAsync(string className, int? subjectId = null);
    }
}
