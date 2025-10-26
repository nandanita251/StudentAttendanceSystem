namespace StudentAttendanceSystem.ViewModels
{
    public class AttendanceReportViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int TotalClasses { get; set; }
        public int PresentClasses { get; set; }
        public int AbsentClasses { get; set; }
        public double AttendancePercentage { get; set; }
    }

    public class StudentAttendanceDetailViewModel
    {
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        
        public List<SubjectAttendanceViewModel> SubjectAttendances { get; set; } = new List<SubjectAttendanceViewModel>();
        
        public double OverallPercentage { get; set; }
    }

    public class SubjectAttendanceViewModel
    {
        public string SubjectName { get; set; } = string.Empty;
        public int TotalClasses { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public double Percentage { get; set; }
    }
}
