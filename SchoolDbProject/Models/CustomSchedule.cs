using System;
using System.Collections.Generic;

namespace SchoolDbProject.Models
{
    public class CustomSchedule : IComparable
    {
        public byte? LessonNumber { get; set; }
        public string Subject { get; set; }
        public byte? DayOfWeek { get; set; }
        public int? CabinetId { get; set; }

        public int CompareTo(object obj)
        {
            CustomSchedule schedule = (CustomSchedule)obj;
            if (this.LessonNumber > schedule.LessonNumber)
            {
                return 1;
            }
            else if (this.LessonNumber < schedule.LessonNumber)
            {
                return -1;
            }

            return 0;
        }
    }
}
