﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect Email Format.")]
        public string Email { get; set; }

        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? ClassId { get; set; }

        public virtual Class Class { get; set; }
    }
}