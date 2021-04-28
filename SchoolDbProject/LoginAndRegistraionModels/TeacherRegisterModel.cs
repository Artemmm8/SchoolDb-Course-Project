﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.LoginAndRegistraionModels
{
    public class TeacherRegisterModel
    {
        [Required(ErrorMessage = "This field is required.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress(ErrorMessage = "Incorrect Email Format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch.")]
        public string ConfirmPassword { get; set; }
    }
}
