using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Models
{
    public class CustomCabinet
    {
        [Required(ErrorMessage = "This field is required.")]
        public int? SelectedCabinet { get; set; }

        public List<int?> Cabinets { get; set; }
    }
}
