using SchoolDbProject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        public int ClassId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [ClassNameSpecificFormat(ErrorMessage = "Incorrect class name format.")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public byte? NumberOfStudents { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
