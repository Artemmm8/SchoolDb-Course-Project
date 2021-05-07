using SchoolDbProject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class Subject
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [OnlyLetters(ErrorMessage = "Incorrect subject name format. Only letters allowed.")]
        public string SubjectName { get; set; }
    }
}
