using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomTeacherMarks
    {
        public List<string> Classes { get; set; }
        public List<string> Students { get; set; }
        public List<string> Subjects { get; set; }
        public List<byte?> Marks { get; set; }
        public string SelectedClass { get; set; }
        public string SelectedStudent { get; set; }
        public string SelectedSubject { get; set; }
        public byte? SelectedMark { get; set; }
    }
}
