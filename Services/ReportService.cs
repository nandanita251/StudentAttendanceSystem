using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Data;
using StudentAttendanceSystem.ViewModels;
using ClosedXML.Excel;

namespace StudentAttendanceSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceReportViewModel>> GetClassAttendanceReportAsync(
            string className, int? subjectId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .Where(a => a.Student.Class == className);

            if (subjectId.HasValue)
                query = query.Where(a => a.SubjectId == subjectId.Value);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            var attendanceData = await query.ToListAsync();

            var report = attendanceData
                .GroupBy(a => a.StudentId)
                .Select(g => new AttendanceReportViewModel
                {
                    StudentId = g.Key,
                    StudentName = g.First().Student.Name,
                    RollNo = g.First().Student.RollNo,
                    Class = g.First().Student.Class,
                    TotalClasses = g.Count(),
                    PresentClasses = g.Count(a => a.Status),
                    AbsentClasses = g.Count(a => !a.Status),
                    AttendancePercentage = g.Count() > 0 ? Math.Round((double)g.Count(a => a.Status) / g.Count() * 100, 2) : 0
                })
                .OrderBy(r => r.RollNo)
                .ToList();

            return report;
        }

        public async Task<StudentAttendanceDetailViewModel> GetStudentAttendanceDetailAsync(
            int studentId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return new StudentAttendanceDetailViewModel();

            var query = _context.Attendances
                .Include(a => a.Subject)
                .Where(a => a.StudentId == studentId);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            var attendanceData = await query.ToListAsync();

            var subjectAttendances = attendanceData
                .GroupBy(a => a.SubjectId)
                .Select(g => new SubjectAttendanceViewModel
                {
                    SubjectName = g.First().Subject.SubjectName,
                    TotalClasses = g.Count(),
                    Present = g.Count(a => a.Status),
                    Absent = g.Count(a => !a.Status),
                    Percentage = g.Count() > 0 ? Math.Round((double)g.Count(a => a.Status) / g.Count() * 100, 2) : 0
                })
                .ToList();

            var totalClasses = attendanceData.Count;
            var totalPresent = attendanceData.Count(a => a.Status);

            return new StudentAttendanceDetailViewModel
            {
                StudentName = student.Name,
                RollNo = student.RollNo,
                Class = student.Class,
                SubjectAttendances = subjectAttendances,
                OverallPercentage = totalClasses > 0 ? Math.Round((double)totalPresent / totalClasses * 100, 2) : 0
            };
        }

        public async Task<byte[]> ExportAttendanceToExcelAsync(string className, int? subjectId = null)
        {
            var report = await GetClassAttendanceReportAsync(className, subjectId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Attendance Report");

            worksheet.Cell(1, 1).Value = "Roll No";
            worksheet.Cell(1, 2).Value = "Student Name";
            worksheet.Cell(1, 3).Value = "Class";
            worksheet.Cell(1, 4).Value = "Total Classes";
            worksheet.Cell(1, 5).Value = "Present";
            worksheet.Cell(1, 6).Value = "Absent";
            worksheet.Cell(1, 7).Value = "Percentage";

            var headerRange = worksheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            int row = 2;
            foreach (var item in report)
            {
                worksheet.Cell(row, 1).Value = item.RollNo;
                worksheet.Cell(row, 2).Value = item.StudentName;
                worksheet.Cell(row, 3).Value = item.Class;
                worksheet.Cell(row, 4).Value = item.TotalClasses;
                worksheet.Cell(row, 5).Value = item.PresentClasses;
                worksheet.Cell(row, 6).Value = item.AbsentClasses;
                worksheet.Cell(row, 7).Value = item.AttendancePercentage + "%";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
