using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Class { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string RollNo { get; set; } = string.Empty;

        public string? UserId { get; set; }
        
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
