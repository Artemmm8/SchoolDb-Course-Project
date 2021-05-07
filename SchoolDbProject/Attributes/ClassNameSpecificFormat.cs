using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolDbProject.Attributes
{
    public class ClassNameSpecificFormat : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string name = (string)value;
            string pattern = "[1-9]+[a-z]$";
            if (name != null && Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase) && name.Length >=1 && name.Length <= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
