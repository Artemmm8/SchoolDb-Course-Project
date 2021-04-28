namespace SchoolDbProject.Models
{
    public class CustomSchedule
    {
        public byte? LessonNumber { get; set; }
        public string Subject { get; set; }
        public byte? DayOfWeek { get; set; }
        public int? CabinetId { get; set; }
    }
}
