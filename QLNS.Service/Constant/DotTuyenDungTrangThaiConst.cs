using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.Constant
{
    public class DotTuyenDungTrangThaiConst
    {
        [DisplayName("Mới tạo")]
        public static string MoiTao
        {
            get { return "MOITAO"; }
            private set { }
        }
        [DisplayName("Đã duyệt")]
        public static string DaDuyet
        {
            get { return "DADUYET"; }
            private set { }
        }
    }
}
