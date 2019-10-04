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
    public class SpecialCharacterAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public SpecialCharacterAttribute(string pattern = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,") : base(pattern)
        {
            ErrorMessage = "Vui lòng không nhập các ký tự đặc biệt";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            string input = (string)value;
            if (input.ToCharArray().Intersect(specialChar.ToArray()).Any())
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            KeyValuePair<string, object>[] args = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string,object>("pattern",this.Pattern)
            };
            var rule = new CustomModelClientValidationRule(FormatErrorMessage(metadata.GetDisplayName()), "regexcustomchars", args);
            yield return rule;
        }
    }
}
