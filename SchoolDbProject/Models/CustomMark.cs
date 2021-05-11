using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomMark
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedMark { get; set; }

        public List<string> Marks { get; set; }
    }
}
