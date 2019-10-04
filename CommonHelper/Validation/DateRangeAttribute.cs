using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper.Validation
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string mininumDate) : base(typeof(DateTime), mininumDate, string.Format("{0:dd/MM/yyyy}", DateTime.Now))
        {
            ErrorMessage = "Vui lòng nhập ngày nhỏ hơn hiện tại";
        }
    }
}
