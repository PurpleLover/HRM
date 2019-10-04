using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CommonHelper.Validation
{
    public class NumericAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public NumericAttribute(string pattern = "([0-9]+)") : base(pattern)
        {
            ErrorMessage = "Vui lòng nhập định dạng số";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || Regex.IsMatch((string)value, this.Pattern))
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
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "regexnumeric", args);
            yield return rule;
        }
    }
}
