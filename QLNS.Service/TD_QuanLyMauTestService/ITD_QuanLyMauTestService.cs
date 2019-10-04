using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.TD_QuanLyMauTestService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLNS.Service.TD_QuanLyMauTestService
{
    public interface ITD_QuanLyMauTestService : IEntityService<TD_QuanLyMauTest>
    {
        PageListResultBO<QuanLyMauTestDTO> GetDaTaByPage(QuanLyMauTestSearchDTO searchModel, int pageIndex = 1, int pageSize = 20);
        JsonResultBO SaveTemplateFile(SaveFileModel saveFileModel, string category);
    }
}
