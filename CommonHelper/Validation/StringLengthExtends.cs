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
    public class StringLengthExtends : StringLengthAttribute, IClientValidatable
    {
        public StringLengthExtends(int maximumLength) : base(maximumLength)
        {
            ErrorMessage = string.Format("Vui lòng nhập ít hơn {0} ký tự", maximumLength);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (value as string).Length <= this.MaximumLength)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            KeyValuePair<string, object>[] args = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("maxlength",this.MaximumLength)
            };
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "stringlengthextend", args);
            yield return rule;
        }
    }
}
