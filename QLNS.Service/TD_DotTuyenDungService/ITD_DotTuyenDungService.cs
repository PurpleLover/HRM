using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentRequestService.DTO;
using QLNS.Service.TD_DotTuyenDungService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.TD_DotTuyenDungService
{
    public interface ITD_DotTuyenDungService : IEntityService<TD_DotTuyenDung>
    {
        PageListResultBO<DotTuyenDungDTO> GetDaTaByPage(DotTuyenDungSearchDTO searchModel, int pageIndex = 1, int pageSize = 20);
        List<SelectListItem> DropdownListDotTuyenDung(int? selected = 0);
        List<RecruitmentRequestDTO> GetYeuCauCuaDotTuyenDung(int dotTuyenDungID);
        bool SaveYeuCauOfDotTuyenDung(List<int> dsYeuCauId, int dotTuyenDungId);
        List<SelectListItem> GetYeuCauCuaDotTuyenDungDropdownlist(int dotTuyenDungID, int? selectedId = null);
    }
}
