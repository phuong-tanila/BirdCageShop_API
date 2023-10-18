using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Commons.ValidatorAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class FirebaseUrl : ValidationAttribute
    {
        public string FirebaseFileUrlTemplate { get; set; }

        public FirebaseUrl(string firebaseFileUrlTemplate)
        {
            FirebaseFileUrlTemplate = firebaseFileUrlTemplate;
        }

        public FirebaseUrl()
        {

            //FirebaseFileUrlTemplate = config["firebaseFileUrlTemplate"];
            //this.
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if(FirebaseFileUrlTemplate is null)
            {
                var config = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
                FirebaseFileUrlTemplate = config["FirebaseFileUrlTemplate"];
            }
            if (
                value is string valueAsString &&
                valueAsString.StartsWith(FirebaseFileUrlTemplate, StringComparison.OrdinalIgnoreCase)
            )
                return ValidationResult.Success;
            return new ValidationResult("Invalid image url");

        }
    }
}
