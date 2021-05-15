using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolDbProject.Attributes
{
    public class PhoneNumberBel : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string name = (string)value;
            string pattern = @"\+[0-9]{3}\([0-9]{2}\)[0-9]{3}-[0-9]{2}-[0-9]{2}";
            if (name != null && Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase) && name.Length <= 255)
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
