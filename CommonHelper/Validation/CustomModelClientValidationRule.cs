using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CommonHelper.Validation
{
    public class CustomModelClientValidationRule : ModelClientValidationRule
    {
        public CustomModelClientValidationRule(
            string errorMessage,
            string pastOnlyValidateType,
            params KeyValuePair<string, object>[] args)
        {
            ValidationType = pastOnlyValidateType;
            ErrorMessage = errorMessage;
            if(args != null && args.Length > 0)
            {
                foreach(var item in args)
                {
                    ValidationParameters.Add(item.Key, item.Value);
                }
            }
        }
    }
}
