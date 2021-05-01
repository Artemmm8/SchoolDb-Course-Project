using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomStudent
    {
        public string StudentId { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect Email Format.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ClassName { get; set; }
        public List<string> Classes { get; set; }
    }
}
