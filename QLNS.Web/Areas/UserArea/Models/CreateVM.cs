using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.UserArea.Models
{
    public class CreateVM
    {
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
    }
}