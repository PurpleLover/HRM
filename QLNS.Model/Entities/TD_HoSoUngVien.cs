using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Entities
{
    [Table("TD_HoSoUngVien")]
    public class TD_HoSoUngVien : AuditableEntity<int>
    {
        public int? IDDotTuyenDung { get; set; }
        public int? IdYeuCauTuyenDung { get; set; }
        public DateTime NgayNopHoSo { get; set; }
        [Required]
        [StringLength(250)]
        public string HoTen { get; set; }

        [StringLength(250)]
        public string KenhUngTuyen { get; set; }

        [StringLength(50)]
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        [StringLength(250)]
        public string DienThoai { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(500)]
        public string DiaChiHienTai { get; set; }
        [StringLength(50)]
        public string CMND { get; set; }
        public DateTime? NgayCapCMND { get; set; }
        [StringLength(500)]
        public string   NoiCapCMND { get; set; }
        [StringLength(500)]
        public string NoiSinh { get; set; }
        [StringLength(100)]
        public string DanToc { get; set; }
        [StringLength(100)]
        public string TonGiao { get; set; }
        [StringLength(100)]
        public string QuocTich { get; set; }
        [StringLength(100)]
        public string QueQuan { get; set; }
        
        public string DiaChiThuongTru { get; set; }
        [StringLength(250)]
        public string XuatThan { get; set; }
        public decimal? ThuNhapGanNhat { get; set; }
        public decimal? ThuNhapMongMuon { get; set; }
        public string GhiChu { get; set; }
        public string Avatar { get; set; }
        public string TrangThai { get; set; }

        public string LyDoUngTuyen { get; set; }
        [StringLength(250)]
        public string NguoiGioiThieu { get; set; }
        
    }
}
