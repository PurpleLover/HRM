using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS
{
    public class PermissionCodeConst
    {

        [DisplayName("Đợt tuyển dụng")]
        public static string DotTuyenDung { get; set; } = "DotTuyenDung";
        [DisplayName("Dashboard")]
        public static string Dashboard { get; set; } = "Dashboard";
        [DisplayName("Quản  lý tài khoản")]
        public static string QLTaiKhoan { get; set; } = "QLTaiKhoan";

        [DisplayName("Quản lý chức năng")]
        public static string QLChucNang { get; set; } = "QLChucNang";
        [DisplayName("Hồ sơ ứng viên")]
        public static string HsUngVien { get; set; } = "HsUngVien";
        [DisplayName("Quản lý vai trò")]
        public static string QLVaiTro { get; set; } = "QLVaiTro";

        [DisplayName("Quản lý kỹ năng")]
        public static string QL_KYNANG { get; set; } = "QL_KYNANG";
        [DisplayName("Quản lý nhóm kỹ năng")]
        public static string QL_NHOM_KYNANG { get; set; } = "QL_NHOM_KYNANG";
        [DisplayName("Yêu cầu tuyển dụng")]
        public static string YEUCAU_TUYENDUNG { get; set; } = "YEUCAU_TUYENDUNG";
        [DisplayName("Quản lý danh mục")]
        public static string QLDanhMuc { get; set; } = "QLDanhMuc";
        [DisplayName("Quản lý phòng ban")]
        public static string QLPhongBan { get; set; } = "QLPhongBan";
        [DisplayName("Quản lý cấu hình chung")]
        public static string QLCauHinhChung { get; set; } = "QLCauHinhChung";


    }
}
