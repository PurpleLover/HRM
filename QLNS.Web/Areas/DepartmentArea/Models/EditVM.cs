using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DepartmentArea.Models
{
    public class EditVM
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thông tin này!")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Code { get; set; }

        public long? ParentId { get; set; }

        public long Type { get; set; }

        public int Level { get; set; }
    }
}