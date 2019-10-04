using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.OperationRepository;
using QLNS.Service.Common;
using QLNS.Service.OperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using PagedList;
using QLNS.Repository.UserRoleRepository;
using QLNS.Repository.ModuleRepository;
using QLNS.Repository.RoleRepository;
using QLNS.Repository.RoleOperationRepository;
using QLNS.Service.ModuleService.DTO;

namespace QLNS.Service.OperationService
{
    public class OperationService : EntityService<Operation>, IOperationService
    {
        IUnitOfWork _unitOfWork;
        IOperationRepository _operationRepository;
        IUserRoleRepository _userRoleRepository;
        IModuleRepository _moduleRepository;
        IRoleRepository _roleRepository;
        IRoleOperationRepository _roleOperationRepository;
        ILog _loger;

        public OperationService(IUnitOfWork unitOfWork, IOperationRepository operationRepository, ILog loger,
            IUserRoleRepository userRoleRepository,
            IModuleRepository moduleRepository,
            IRoleRepository roleRepository,
            IRoleOperationRepository roleOperationRepository
            ) :
            base(unitOfWork, operationRepository)
        {
            _roleOperationRepository = roleOperationRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _operationRepository = operationRepository;
            _loger = loger;
            _loger.Info("Khởi tạo operation service");
        }

        public PageListResultBO<OperationDTO> GetDataByPage(OperationSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var queryResult = (from operation in this._operationRepository.GetAllAsQueryable()
                               select new OperationDTO()
                               {
                                   Id = operation.Id,
                                   Name = operation.Name,
                                   Code = operation.Code,
                                   URL = operation.URL,
                                   IsShow = operation.IsShow,
                                   ModuleId = operation.ModuleId,
                                   Order = operation.Order
                               });
            if (searchParams != null)
            {
                queryResult = queryResult.Where(x => x.ModuleId == searchParams.QueryModuleId);
                if (!string.IsNullOrEmpty(searchParams.QueryName))
                {
                    searchParams.QueryName = searchParams.QueryName.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Name.Trim().ToLower().Contains(searchParams.QueryName));
                }
                if (searchParams.QueryIsShow != null)
                {
                    queryResult = queryResult.Where(x => x.IsShow == searchParams.QueryIsShow.Value);
                }

                if (!string.IsNullOrEmpty(searchParams.sortQuery))
                {
                    queryResult = queryResult.OrderBy(searchParams.sortQuery);
                }
                else
                {
                    queryResult = queryResult.OrderBy(x => x.Order)
                        .ThenBy(x => x.Name);
                }
            }
            else
            {
                queryResult = queryResult.OrderBy(x => x.Order)
                        .ThenBy(x => x.Name);
            }

            var result = new PageListResultBO<OperationDTO>();
            if (pageSize == -1)
            {
                var pagedList = queryResult.ToList();
                result.Count = pagedList.Count;
                result.TotalPage = 1;
                result.ListItem = pagedList;
            }
            else
            {
                var dataPageList = queryResult.ToPagedList(pageIndex, pageSize);
                result.Count = dataPageList.TotalItemCount;
                result.TotalPage = dataPageList.PageCount;
                result.ListItem = dataPageList.ToList();
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách thao tác theo User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public List<ModuleMenuDTO> GetListOperationOfUser(long userId)
        {
            var listOperation = new List<ModuleMenuDTO>();
            var listRoleIdOfUser = (from userRole in _userRoleRepository.GetAllAsQueryable().Where(x => x.UserId == userId)
                                    join role in _roleRepository.GetAllAsQueryable()
                                    on userRole.RoleId equals role.Id
                                    select role).Select(x => x.Id).ToList();
            if (listRoleIdOfUser != null && listRoleIdOfUser.Any())
            {
                listOperation = (from operationId in _roleOperationRepository.GetAllAsQueryable().Where(x => x.IsAccess == 1 && listRoleIdOfUser.Contains(x.RoleId)).Select(x => x.OperationId)
                                 join operation in _operationRepository.GetAllAsQueryable() on operationId equals operation.Id
                                 group operation by operation.ModuleId into groupMenu
                                 join module in _moduleRepository.GetAllAsQueryable() on groupMenu.Key equals module.Id
                                 select new ModuleMenuDTO()
                                 {
                                     Id = groupMenu.Key,
                                     ClassCss = module.ClassCss,
                                     Code = module.Code,
                                     CreatedBy = module.CreatedBy,
                                     Icon = module.Icon,
                                     CreatedDate = module.CreatedDate,
                                     IsShow = module.IsShow,
                                     Link = module.Link,
                                     Name = module.Name,
                                     Order = module.Order,
                                     StyleCss = module.StyleCss,
                                     UpdatedBy = module.UpdatedBy,
                                     UpdatedDate = module.UpdatedDate,
                                     ListOperation = groupMenu.OrderBy(x => x.Order).ThenBy(x => x.Id).ToList()
                                 })
                                   .ToList();
            }
            return listOperation;
        }

        public bool CheckCode(string code, long? id = null) => _operationRepository.GetAllAsQueryable().Any(x => x.Code != null && x.Code == code && (id.HasValue ? x.Id != id : true));


    }
}
