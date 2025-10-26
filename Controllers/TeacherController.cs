using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Data;
using StudentAttendanceSystem.Models;
using StudentAttendanceSystem.Services;
using StudentAttendanceSystem.ViewModels;

namespace StudentAttendanceSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAttendanceService _attendanceService;
        private readonly IReportService _reportService;

        public TeacherController(ApplicationDbContext context, UserManager<User> userManager, 
            IAttendanceService attendanceService, IReportService reportService)
        {
            _context = context;
            _userManager = userManager;
            _attendanceService = attendanceService;
            _reportService = reportService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user!.Id);

            if (teacher != null)
            {
                ViewBag.TeacherName = teacher.Name;
                ViewBag.TotalSubjects = await _context.Subjects.Where(s => s.TeacherId == teacher.TeacherId).CountAsync();
                ViewBag.TodayAttendance = await _context.Attendances
                    .Where(a => a.Date.Date == DateTime.Today && a.Subject.TeacherId == teacher.TeacherId)
                    .CountAsync();
            }

            var subjects = await _context.Subjects
                .Include(s => s.Teacher)
                .Where(s => s.TeacherId == teacher!.TeacherId)
                .ToListAsync();

            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> MarkAttendance(int subjectId)
        {
            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(s => s.SubjectId == subjectId);

            if (subject == null)
                return NotFound();

            var students = await _attendanceService.GetStudentsByClassAsync(subject.Class);

            var model = new MarkAttendanceViewModel
            {
                SubjectId = subjectId,
                Date = DateTime.Today,
                SubjectName = subject.SubjectName,
                Class = subject.Class,
                Students = students.Select(s => new AttendanceViewModel
                {
                    StudentId = s.StudentId,
                    StudentName = s.Name,
                    RollNo = s.RollNo,
                    IsPresent = true
                }).ToList()
            };

            var isMarked = await _attendanceService.IsAttendanceMarkedAsync(subjectId, DateTime.Today);
            if (isMarked)
            {
                var existingAttendance = await _attendanceService.GetAttendanceByDateAsync(subjectId, DateTime.Today);
                foreach (var attendance in existingAttendance)
                {
                    var studentVM = model.Students.FirstOrDefault(s => s.StudentId == attendance.StudentId);
                    if (studentVM != null)
                    {
                        studentVM.IsPresent = attendance.Status;
                        studentVM.Remarks = attendance.Remarks;
                    }
                }
                ViewBag.AlreadyMarked = true;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _attendanceService.MarkAttendanceAsync(model);
                if (result)
                {
                    TempData["Success"] = "Attendance marked successfully!";
                    return RedirectToAction(nameof(Dashboard));
                }
                ModelState.AddModelError("", "Failed to mark attendance.");
            }

            var subject = await _context.Subjects.FindAsync(model.SubjectId);
            if (subject != null)
            {
                model.SubjectName = subject.SubjectName;
                model.Class = subject.Class;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAttendance(int subjectId, DateTime? date)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null)
                return NotFound();

            ViewBag.SubjectName = subject.SubjectName;
            ViewBag.Class = subject.Class;
            ViewBag.Date = date ?? DateTime.Today;

            var attendanceRecords = await _attendanceService.GetAttendanceByDateAsync(subjectId, date ?? DateTime.Today);
            
            return View(attendanceRecords);
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user!.Id);

            if (teacher == null)
                return NotFound();

            var subjects = await _context.Subjects
                .Where(s => s.TeacherId == teacher.TeacherId)
                .ToListAsync();

            ViewBag.Subjects = subjects;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(int subjectId, DateTime? startDate, DateTime? endDate)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null)
                return NotFound();

            var report = await _reportService.GetClassAttendanceReportAsync(subject.Class, subjectId, startDate, endDate);
            
            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user!.Id);
            var subjects = await _context.Subjects.Where(s => s.TeacherId == teacher!.TeacherId).ToListAsync();
            
            ViewBag.Subjects = subjects;
            ViewBag.SelectedSubject = subjectId;
            ViewBag.SubjectName = subject.SubjectName;

            return View("Reports", report);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(int subjectId)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null)
                return NotFound();

            var fileBytes = await _reportService.ExportAttendanceToExcelAsync(subject.Class, subjectId);
            var fileName = $"Attendance_{subject.SubjectName}_{DateTime.Now:yyyyMMdd}.xlsx";
            
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
