using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomLesson
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedLesson { get; set; }

        public List<string> Lessons { get; set; }
    }
}
