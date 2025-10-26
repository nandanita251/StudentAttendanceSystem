using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceSystem.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
