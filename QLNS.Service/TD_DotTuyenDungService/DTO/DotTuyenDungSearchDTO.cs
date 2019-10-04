using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_DotTuyenDungService.DTO
{
    public class DotTuyenDungSearchDTO:SearchBase
    {
        public string seaTenDot { get; set; }
        public DateTime? ngaybatdaufrom { get; set; }
        public DateTime? ngaybatdauto { get; set; }
        public DateTime? ngayketthucfrom { get; set; }
        public DateTime? ngayketthucto { get; set; }
        public string seaTrangThai { get; set; }
    }
}
