using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DmDulieuDanhmucArea.Models
{
    public class CreateVM
    {
        public long? GroupId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Code { get; set; }

        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Note { get; set; }
        [Range(0, 100, ErrorMessage = "Thứ tự phải nằm trong khoảng 0 đến 100")]
        public int? Priority { get; set; }
    }
}