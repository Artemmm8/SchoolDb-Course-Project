using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.LoginAndRegistraionModels
{
    public class StudentLoginModel
    {
        [Required(ErrorMessage = "This field is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
