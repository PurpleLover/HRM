using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.ModuleRepository;
using QLNS.Repository.OperationRepository;
using QLNS.Repository.RoleOperationRepository;
using QLNS.Repository.RoleRepository;
using QLNS.Service.ModuleService.DTO;
using QLNS.Service.OperationService.DTO;
using QLNS.Service.RoleOperationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.RoleOperationService
{
    public class RoleOperationService : EntityService<RoleOperation>, IRoleOperationService
    {
        IRoleOperationRepository _roleOperationRepository;
        IModuleRepository _moduleRepository;
        IRoleRepository _roleRepository;
        IOperationRepository _operationRepository;
        ILog _ilog;

        public RoleOperationService(IUnitOfWork unitOfWork,
            IRoleOperationRepository roleOperationRepository,
            IModuleRepository moduleRepository,
            IRoleRepository roleRepository,
            IOperationRepository operationRepository,
            ILog logger)
            : base(unitOfWork, roleOperationRepository)
        {
            _roleOperationRepository = roleOperationRepository;
            _moduleRepository = moduleRepository;
            _roleRepository = roleRepository;
            _operationRepository = operationRepository;
            _ilog = logger;
            logger.Info("Khởi tạo RoleOperation Service");
        }

        /// <summary>
        /// @author:duynn
        /// @description: lấy danh sách phân quyền vai trò
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public RoleOperationDTO GetConfigureOperation(int roleId)
        {
            var queryRoleOperation = this._roleOperationRepository.GetAllAsQueryable()
                .Where(x => x.RoleId == roleId);
            var queryAllModules = this._moduleRepository.GetAllAsQueryable();
            var queryAllOperations = this._operationRepository.GetAllAsQueryable();

            var result = (from role in this._roleRepository.GetAllAsQueryable()
                          .Where(x => x.Id == roleId)
                          select new RoleOperationDTO()
                          {
                              RoleId = role.Id,
                              RoleName = role.Name,
                              GroupModules = (from module in queryAllModules
                                              join operation in queryAllOperations
                                              on module.Id equals operation.ModuleId
                                              into groupModuleOperation
                                              select new ModuleDTO()
                                              {
                                                  Id = module.Id,
                                                  Name = module.Name,
                                                  GroupOperations = groupModuleOperation.Select(y => new OperationDTO()
                                                  {
                                                      Id = y.Id,
                                                      Name = y.Name,
                                                      IsAccess = queryRoleOperation.Where(x => x.OperationId == y.Id && x.IsAccess > 0).Any()
                                                  }).AsEnumerable()
                                              }).AsEnumerable()
                          }).FirstOrDefault();
            return result;
        }
    }
}
