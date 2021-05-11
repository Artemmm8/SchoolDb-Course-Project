using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomStudentDel
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedStudent { get; set; }

        public List<string> Students { get; set; }
    }
}
