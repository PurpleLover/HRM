using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.TDHoSoUngVienArea.Models
{
    public class EditHoSoUngVienModel
    {
        public long Id { get; set; }
        public int? IDDotTuyenDung { get; set; }
        public int? IdYeuCauTuyenDung { get; set; }
        public DateTime NgayNopHoSo { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập thông tin này")]
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
        public string NoiCapCMND { get; set; }
        [StringLength(500)]
        public string NoiSinh { get; set; }
        [StringLength(100)]
        
        [Required(ErrorMessage = "Vui lòng nhập thông tin này")]
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
        public string LyDoUngTuyen { get; set; }
        [StringLength(250)]
        public string NguoiGioiThieu { get; set; }

    }
}