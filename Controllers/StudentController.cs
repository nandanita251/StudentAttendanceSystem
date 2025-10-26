using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Data;
using StudentAttendanceSystem.Models;
using StudentAttendanceSystem.Services;

namespace StudentAttendanceSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IReportService _reportService;

        public StudentController(ApplicationDbContext context, UserManager<User> userManager, IReportService reportService)
        {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
                return NotFound();

            var attendanceDetail = await _reportService.GetStudentAttendanceDetailAsync(student.StudentId);

            ViewBag.StudentName = student.Name;
            ViewBag.RollNo = student.RollNo;
            ViewBag.Class = student.Class;

            return View(attendanceDetail);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAttendance(DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
                return NotFound();

            var attendanceDetail = await _reportService.GetStudentAttendanceDetailAsync(student.StudentId, startDate, endDate);

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(attendanceDetail);
        }

        [HttpGet]
        public async Task<IActionResult> SubjectWiseAttendance()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
                return NotFound();

            var attendanceDetail = await _reportService.GetStudentAttendanceDetailAsync(student.StudentId);

            return View(attendanceDetail.SubjectAttendances);
        }

        [HttpGet]
        public async Task<IActionResult> AttendanceHistory(int? subjectId, int page = 1, int pageSize = 20)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user!.Id);

            if (student == null)
                return NotFound();

            var query = _context.Attendances
                .Include(a => a.Subject)
                .Where(a => a.StudentId == student.StudentId)
                .OrderByDescending(a => a.Date);

            if (subjectId.HasValue)
                query = (IOrderedQueryable<Attendance>)query.Where(a => a.SubjectId == subjectId.Value);

            var totalRecords = await query.CountAsync();
            var attendanceRecords = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            ViewBag.Subjects = await _context.Subjects.ToListAsync();
            ViewBag.SelectedSubject = subjectId;

            return View(attendanceRecords);
        }
    }
}
