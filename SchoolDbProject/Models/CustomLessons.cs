using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomLessons
    {
        public List<string> Classes { get; set; }
        public List<string> Subjects { get; set; }
        public List<string> Teachers { get; set; } 
        public List<int?> Cabinets { get; set; }
        public List<byte?> LessonNumbers { get; set; }
        public List<byte?> DaysOfWeek { get; set; }
        public string SelectedClass { get; set; }
        public string SelectedSubject { get; set; }
        public string SelectedTeacher { get; set; }
        public int? SelectedCabinet { get; set; }
        public byte? SelectedLessonNumber { get; set; }
        public byte? SelectedDayOfWeek { get; set; }
    }
}
