using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DmNhomDanhmucArea.Models
{
    public class CreateVM
    {
        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MaxLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MaxLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        public string GroupCode { get; set; }
    }
}