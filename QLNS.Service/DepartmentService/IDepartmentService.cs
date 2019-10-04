using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DepartmentService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.DepartmentService
{
    public interface IDepartmentService : IEntityService<Department>
    {  
        /// <summary>
        /// Kiểm tra mã đã tồn tại hay chưa?
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCodeExisted(string code);
        /// <summary>
        /// Lấy cây phòng ban
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        DepartmentDTO GetTree(long departmentId = 0);
        /// <summary>
        /// Lấy các nhánh và lá của cây
        /// </summary>
        /// <param name="node">Nút gốc</param>
        /// <param name="rawList">Danh sách ban đầu</param>
        void _GetChildOfTree(ref DepartmentDTO node, List<DepartmentDTO> rawList);
        /// <summary>
        /// Lấy danh sách ban đầu, theo thứ tự hiển thị
        /// </summary>
        /// <returns></returns>
        List<DepartmentDTO> _GetAllItems();
    }
}
