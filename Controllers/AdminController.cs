using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceSystem.Data;
using StudentAttendanceSystem.Models;
using StudentAttendanceSystem.Services;

namespace StudentAttendanceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IReportService _reportService;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, IReportService reportService)
        {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalStudents = await _context.Students.CountAsync();
            ViewBag.TotalTeachers = await _context.Teachers.CountAsync();
            ViewBag.TotalSubjects = await _context.Subjects.CountAsync();
            ViewBag.TodayAttendance = await _context.Attendances
                .Where(a => a.Date.Date == DateTime.Today)
                .CountAsync();

            return View();
        }

        public async Task<IActionResult> Students()
        {
            var students = await _context.Students.OrderBy(s => s.Class).ThenBy(s => s.RollNo).ToListAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student created successfully!";
                return RedirectToAction(nameof(Students));
            }
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student updated successfully!";
                return RedirectToAction(nameof(Students));
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student deleted successfully!";
            }
            return RedirectToAction(nameof(Students));
        }

        public async Task<IActionResult> Teachers()
        {
            var teachers = await _context.Teachers.ToListAsync();
            return View(teachers);
        }

        [HttpGet]
        public IActionResult CreateTeacher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Teacher created successfully!";
                return RedirectToAction(nameof(Teachers));
            }
            return View(teacher);
        }

        [HttpGet]
        public async Task<IActionResult> EditTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Teacher updated successfully!";
                return RedirectToAction(nameof(Teachers));
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Teacher deleted successfully!";
            }
            return RedirectToAction(nameof(Teachers));
        }

        public async Task<IActionResult> Subjects()
        {
            var subjects = await _context.Subjects.Include(s => s.Teacher).ToListAsync();
            return View(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubject()
        {
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject created successfully!";
                return RedirectToAction(nameof(Subjects));
            }
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View(subject);
        }

        [HttpGet]
        public async Task<IActionResult> EditSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Update(subject);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject updated successfully!";
                return RedirectToAction(nameof(Subjects));
            }
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Subject deleted successfully!";
            }
            return RedirectToAction(nameof(Subjects));
        }

        public async Task<IActionResult> Reports()
        {
            ViewBag.Classes = await _context.Students.Select(s => s.Class).Distinct().ToListAsync();
            ViewBag.Subjects = await _context.Subjects.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReport(string className, int? subjectId, DateTime? startDate, DateTime? endDate)
        {
            var report = await _reportService.GetClassAttendanceReportAsync(className, subjectId, startDate, endDate);
            ViewBag.Classes = await _context.Students.Select(s => s.Class).Distinct().ToListAsync();
            ViewBag.Subjects = await _context.Subjects.ToListAsync();
            ViewBag.SelectedClass = className;
            return View("Reports", report);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string className, int? subjectId)
        {
            var fileBytes = await _reportService.ExportAttendanceToExcelAsync(className, subjectId);
            var fileName = $"Attendance_Report_{className}_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
