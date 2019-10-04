using QLNS.Model.Entities;
using QLNS.Service.RecruitmentRequestService.DTO;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNS.Web.Areas.DotTuyenDungArea.Models
{
    public class DetailViewModel
    {
        public TD_DotTuyenDung dotTuyenDung { get; set; }
        public List<RecruitmentRequestDTO> listYeuCauTuyenDungDot { get; set; }
        public List<HoSoUngVienDTO> dsUngVien { get; set; }
        public List<TaiLieuDinhKem> taiLieuDinhKem { get; set; }
    }
}