using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TaiLieuDinhKem")]
    public class TaiLieuDinhKem : AuditableEntity<long>
    {
        public long? KichThuoc { get; set; }
        public DateTime? NgayPhatHanh { get; set; }
        [Required]
        [StringLength(500)]
        public string TenTaiLieu { get; set; }
        [StringLength(250)]

        public string LoaiTaiLieu { get; set; }
        public long? Item_ID { get; set; }
        public string MoTa { get; set; }
        public string DuongDanFile { get; set; }
        [StringLength(250)]
        public string DinhDangFile { get; set; }
        public int SoLuongDownload { get; set; }
        public long? UserId { get; set; }
    }
}
