using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomTeacher
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedTeacher { get; set; }

        public List<string> Teachers { get; set; }
    }
}
