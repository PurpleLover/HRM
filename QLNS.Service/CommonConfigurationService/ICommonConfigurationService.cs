using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.CommonConfigurationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.CommonConfigurationService
{
    public interface ICommonConfigurationService : IEntityService<CommonConfiguration>
    {
        PageListResultBO<CommonConfigurationDTO> GetDataByPage(CommonConfigurationSearchDTO searchParams, int pageIndex = 1, int pageSize = 20);

        /// <summary>
        /// Lấy dữ liệu bằng mã
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetDataByCode(string code);
       
        /// <summary>
        /// Kiểm tra mã đã tồn tại hay chưa?
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCodeExisted(string code);
    }
}
