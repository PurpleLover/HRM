using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_DotTuyenDung")]
    public class TD_DotTuyenDung : AuditableEntity<int>
    {
        [Required]
        [StringLength(250)]
        public string TenDot { get; set; }

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }

    }
}
