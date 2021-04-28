using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class StudentSchedule
    {
        public byte? LessonNumber { get; set; }
        public byte? DayOfWeek { get; set; }
        public int? ClassId { get; set; }
        public int? CabinetId { get; set; }
        public int? SubjectId { get; set; }
        public int? TeacherId { get; set; }

        public virtual Cabinet Cabinet { get; set; }
        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
