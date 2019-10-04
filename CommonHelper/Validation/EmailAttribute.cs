using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace CommonHelper.Validation
{
    public class EmailAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public EmailAttribute(string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z") : base(pattern)
        {
            ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ";
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
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "regexcustom", args);
            yield return rule;
        }
    }
}
