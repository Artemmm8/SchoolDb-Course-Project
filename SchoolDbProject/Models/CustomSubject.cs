using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomSubject
    {
        [Required(ErrorMessage = "This field is required.")]
        public string SelectedSubject { get; set; }

        public List<string> Subjects { get; set; }
    }
}
