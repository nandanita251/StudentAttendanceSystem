using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceSystem.ViewModels
{
    public class AttendanceViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public bool IsPresent { get; set; }
        public string? Remarks { get; set; }
    }

    public class MarkAttendanceViewModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        public string? SubjectName { get; set; }
        public string? Class { get; set; }

        public List<AttendanceViewModel> Students { get; set; } = new List<AttendanceViewModel>();
    }
}
