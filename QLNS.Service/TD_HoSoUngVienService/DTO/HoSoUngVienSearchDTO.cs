using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_HoSoUngVienService.DTO
{
    public class HoSoUngVienSearchDTO:SearchBase
    {
        public string sea_HOTEN { get; set; }
        public string sea_GIOITINH { get; set; }
        public string sea_DIENTHOAI { get; set; }
        public string sea_EMAIL { get; set; }
        public DateTime? sea_NGAYSINH { get; set; }
        public int? sea_DOT_ID { get; set; }
        public int? sea_YEUCAU { get; set; }
    }
}
