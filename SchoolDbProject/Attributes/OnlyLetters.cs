using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolDbProject.Attributes
{
    public class OnlyLetters : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string name = (string)value;
            string pattern = "^[a-z]*$";
            if (name != null && Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase))
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
