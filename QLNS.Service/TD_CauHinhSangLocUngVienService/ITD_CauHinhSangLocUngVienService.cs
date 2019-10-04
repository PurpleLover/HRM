using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.TD_CauHinhSangLocUngVienService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_CauHinhSangLocUngVienService
{
    public interface ITD_CauHinhSangLocUngVienService : IEntityService<TD_CauHinhSangLocUngVien>
    {
        /// <summary>
        /// Lấy danh sách cấu hình sàng lọc theo đợt tuyển dụng, sắp xếp theo thứ tự tăng dần, sử dụng trong Detail đợt tuyển dụng
        /// </summary>
        /// <param name="dotTuyenDungId">Id đợt tuyển dụng</param>
        /// <returns></returns>
        List<CauHinhSangLocUngVienDTO> GetDataByDotTuyenDungId(long dotTuyenDungId = 0);
    }
}
