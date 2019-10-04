using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.DM_DulieuDanhmucService
{
    public interface IDM_DulieuDanhmucService : IEntityService<DM_DulieuDanhmuc>
    {
        PageListResultBO<DM_DulieuDanhmucDTO> GetDataByPage(long danhMucId, DM_DulieuDanhmucSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
        List<DM_DulieuDanhmuc> GetListDataByGroupId(long groupId);
        bool CheckCodeExisted(long? groupId, string code);
        List<DM_DulieuDanhmuc> GetByCodeGroup(string GroupCode);
        List<SelectListItem> GetDropdownlist(string GroupCode, string SelectedValue);
    }
}
