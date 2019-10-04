using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.TdQuanLyMauTestArea.Models
{
    public class CreateVM
    {
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        public string FileName { get; set; }
        public string Category { get; set; }
    }
}