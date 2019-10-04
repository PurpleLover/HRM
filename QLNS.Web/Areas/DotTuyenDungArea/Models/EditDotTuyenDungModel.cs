using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DotTuyenDungArea.Models
{
    public class EditDotTuyenDungModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        [StringLength(250)]
        public string TenDot { get; set; }

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
        [StringLength(50)]
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}