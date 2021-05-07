using SchoolDbProject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class Cabinet
    {
        [Required(ErrorMessage = "This field is required.")]
        [GreaterThanZero(ErrorMessage = "Only positive Id allowed.")]
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public byte? NumberOfSeats { get; set; }
    }
}
