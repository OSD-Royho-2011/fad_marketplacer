using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NCB.Core.Validations
{
    public class CustomIntValidationAttribute : ValidationAttribute
    {
        public int Value { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if((int)value < Value)
            {
                return new ValidationResult($"value must greater than {Value}");
            }

            return ValidationResult.Success;
        }
    }
}
