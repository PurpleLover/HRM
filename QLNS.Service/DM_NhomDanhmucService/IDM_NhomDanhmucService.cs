using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DM_NhomDanhmucService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.DM_NhomDanhmucService
{
    public interface IDM_NhomDanhmucService : IEntityService<DM_NhomDanhmuc>
    {
        PageListResultBO<DM_NhomDanhmucDTO> GetDataByPage(DM_NhomDanhmucSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);
        bool CheckGroupCodeExisted(string groupCode);
        IEnumerable<SelectListItem> GetDataByCode(string code);
    }
}
