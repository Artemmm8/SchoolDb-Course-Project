using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDbProject.Models
{
    public partial class Mark
    {
        public byte? Mark1 { get; set; }
        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public DateTime Date { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
