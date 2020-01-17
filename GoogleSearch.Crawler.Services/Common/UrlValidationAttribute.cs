using GoogleSearch.Crawler.Services.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoogleSearch.Crawler.Services.Common
{
    class UrlValidationAttribute : ValidationAttribute
    {
        public UrlValidationAttribute()
        {


        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            SearchRequest request = (SearchRequest)validationContext.ObjectInstance;
            bool isValid = Uri.TryCreate(request.Url,UriKind.Absolute, out Uri uri);
            if (!isValid)
            {
                return new ValidationResult("Uri is not in correct format");
            }
            return ValidationResult.Success;

        }
    }
}
