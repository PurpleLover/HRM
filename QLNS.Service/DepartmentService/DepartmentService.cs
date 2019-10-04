using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.DepartmentRepository;
using QLNS.Service.Common;
using QLNS.Service.DepartmentService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.DepartmentService
{
    public class DepartmentService : EntityService<Department>, IDepartmentService
    {
        IUnitOfWork _unitOfWork;
        IDepartmentRepository _departmentRepository;
        ILog _loger;

        public DepartmentService(IUnitOfWork unitOfWork, IDepartmentRepository departmentRepository, ILog loger) : base(unitOfWork, departmentRepository)
        {
            _unitOfWork = unitOfWork;
            _departmentRepository = departmentRepository;
            _loger = loger;
            _loger.Info("Khởi tạo Department service");
        }

        public bool CheckCodeExisted(string code)
        {
            return this._departmentRepository.GetAllAsQueryable().Where(x => x.Code.Equals(code)).Any();
        }

        public DepartmentDTO GetTree(long departmentId = 0)
        {
            // the original taken from db
            List<DepartmentDTO> rawList = _GetAllItems();

            DepartmentDTO root = new DepartmentDTO();
            if (departmentId > 0)
            {
                root = rawList.Where(x => x.Id == departmentId).FirstOrDefault();
                if (root != null)
                {
                    _GetChildOfTree(ref root, rawList);
                }
            }
            else
            {
                root = rawList.Where(x => x.ParentId.HasValue == false).FirstOrDefault();
                if (root != null)
                {
                    _GetChildOfTree(ref root, rawList);
                }
            }

            return root;
        }
        public void _GetChildOfTree(ref DepartmentDTO node, List<DepartmentDTO> rawList)
        {
            long id = node.Id;
            var lstChild = rawList.Where(x => x.ParentId == id).ToList();
            if (lstChild.Count > 0)
            {
                node.Child = new List<DepartmentDTO>();
                node.Child.AddRange(lstChild);
                for (int i = 0; i < lstChild.Count; i++)
                {
                    var item = node.Child[i];
                    _GetChildOfTree(ref item, rawList);
                    node.Child[i] = item;
                }
            }
        }
        public List<DepartmentDTO> _GetAllItems()
        {
            return (from tbl in this._departmentRepository.GetAllAsQueryable()
                    select new DepartmentDTO
                    {
                        Id = tbl.Id,
                        ParentId = tbl.ParentId,
                        Name = tbl.Name,
                        Code = tbl.Code,
                        Level = tbl.Level,
                        Priority = tbl.Priority
                    }).OrderBy(x=>x.Priority).ToList();
        }
    }
}
