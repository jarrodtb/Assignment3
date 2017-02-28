using System;
using System.ComponentModel.DataAnnotations;

namespace DateOfBirth
{
    public class MinimumAgeAttribute : ValidationAttribute
        {
            int _minimumAge;
            public MinimumAgeAttribute(int minimumAge)
            {
                _minimumAge = minimumAge;
            }
            public override bool IsValid(object value)
            {
                DateTime date;
                if (value == null)
                {
                    return false;
                }
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_minimumAge) < DateTime.Now;
                 }
                return false;
            }
        }
}
