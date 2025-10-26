using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceSystem.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; } = string.Empty;

        [StringLength(20)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        public int TeacherId { get; set; }

        [StringLength(50)]
        public string Class { get; set; } = string.Empty;

        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
