using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SchoolDbProject.Attributes;

namespace SchoolDbProject.Models
{
    public class CustomStudent
    {
        [Required(ErrorMessage = "This field is required.")]
        [GreaterThanZero(ErrorMessage = "Only positive Id allowed.")]
        public int? StudentId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MicrosoftEmail(ErrorMessage = "Incorrect email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch.")]
        public string ConfirmPassword { get; set; }

        public string ClassName { get; set; }

        public List<string> Classes { get; set; }
    }
}
