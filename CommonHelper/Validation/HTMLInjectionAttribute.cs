using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace CommonHelper.Validation
{
    public class HTMLInjectionAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public HTMLInjectionAttribute(string pattern = @"<[a-z][\s\S]*>") : base(pattern)
        {
            ErrorMessage = "Vui lòng không nhập ký tự HTML";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !Regex.IsMatch((string)value, this.Pattern))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            KeyValuePair<string, object>[] args = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string,object>("pattern",this.Pattern)
            };
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "regexcustomhtml", args);
            yield return rule;
        }
    }
}
