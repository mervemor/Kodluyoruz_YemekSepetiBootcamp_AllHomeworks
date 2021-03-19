using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Data.Context;
using Week2_Homework2.RequestModels;

namespace Week2_Homework2.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MovieTypeValidation : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)//request hakkında genel bilgiler 
        {

            int movieTypeId = (int)value;

            DatabaseContext db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));

            if (db.MovieType.Any(a => a.MovieTypeId == movieTypeId))
            {
                return ValidationResult.Success;
            }

            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.MemberName));
            }

        }
    }
}
