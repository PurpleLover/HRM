using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.Constant
{
    public class YeuCauTuyenDungTrangThaiConst
    {
        [DisplayName("Mới tạo")]
        public static string MoiTao { get; set; } = "MoiTao";
        [DisplayName("Đang tuyển")]
        public static string DangTuyen { get; set; } = "DangTuyen";
        [DisplayName("Tạm dừng")]
        public static string TamDung { get; set; } = "TamDung";
        [DisplayName("Đã đóng")]
        public static string DaDong { get; set; } = "DaDong";
        [DisplayName("Hủy bỏ")]
        public static string HuyBo { get; set; } = "HuyBo";
    }
}
