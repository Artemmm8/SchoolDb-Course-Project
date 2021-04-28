using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.LoginAndRegistraionModels
{
    public class TeacherLoginModel
    {
        [Required(ErrorMessage = "This field is required.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
