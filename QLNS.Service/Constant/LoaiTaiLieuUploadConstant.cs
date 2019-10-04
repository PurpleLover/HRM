using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.Constant
{
    public class LoaiTaiLieuUploadConstant
    {
        [DisplayName("Hồ sơ ứng viên")]
        public static string HoSoUngVien
        {
            get
            {
                return "FileHoSoUngVien";
            }
            private set { }
        }

        [DisplayName("Hồ sơ ứng viên")]
        public static string DotTuyenDung
        {
            get
            {
                return "FileDotTuyenDung";
            }
            private set { }
        }
    }

    public class DataTypeConstant
    {
        [DisplayName("Danh mục")]
        public static int CATEGORY
        {
            get
            {
                return 1;
            }
            private set { }
        }

        [DisplayName("Ghi chú")]
        public static int TEXT
        {
            get
            {
                return 2;
            }
            private set { }
        }

        [DisplayName("Số")]
        public static int NUMBER
        {
            get
            {
                return 3;
            }
            private set { }
        }
    }
}
