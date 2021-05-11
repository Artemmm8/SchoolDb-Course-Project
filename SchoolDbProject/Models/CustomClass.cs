using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomClass
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedClass { get; set; }

        public List<string> Classes { get; set; }
    }
}
