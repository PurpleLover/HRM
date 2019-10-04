using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_DotTuyenDungService.DTO
{
    public class DotTuyenDungDTO : TD_DotTuyenDung
    {
        public string TenTrangThai { get { return ConstantExtension.GetName<DotTuyenDungTrangThaiConst>(TrangThai); } set { } }
        public List<TD_YeuCauVaDotTuyenDung> tD_YeuCauVaDots { get; set; }
        public List<RecruitmentRequest> recruitmentRequests { get; set; }
    }
}
