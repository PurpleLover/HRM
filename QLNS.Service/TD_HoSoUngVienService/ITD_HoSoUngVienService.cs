using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_HoSoUngVienService
{
    public interface ITD_HoSoUngVienService : IEntityService<TD_HoSoUngVien>
    {
        PageListResultBO<HoSoUngVienDTO> GetDaTaByPage(HoSoUngVienSearchDTO searchModel, int pageIndex = 1, int pageSize = 20);
        HoSoUngVienDTO GetInfoById(int id);
        List<HoSoUngVienDTO> GetAllDataInfo();
        List<HoSoUngVienDTO> GetHoSoOfDotTuyenDung(int dotTuyenDungId);
    }
}
