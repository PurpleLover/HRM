using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CommonHelper.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredExtendAttribute : ValidationAttribute, IClientValidatable
    {
        public RequiredExtendAttribute() : base()
        {
            ErrorMessage = "Vui lòng nhập thông tin";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "requiredextend");
            yield return rule;
        }
    }
}
