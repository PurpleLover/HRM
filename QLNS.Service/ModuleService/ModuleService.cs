using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.ModuleRepository;
using QLNS.Service.Common;
using QLNS.Service.ModuleService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using QLNS.Repository.OperationRepository;

namespace QLNS.Service.ModuleService
{
    public class ModuleService : EntityService<Module>, IModuleService
    {
        IUnitOfWork _unitOfWork;
        IModuleRepository _moduleRepository;
        IOperationRepository _operationRepository;
        ILog _loger;

        public ModuleService(IUnitOfWork unitOfWork, 
            IModuleRepository moduleRepository, 
            IOperationRepository operationRepository, ILog loger)
            : base(unitOfWork, moduleRepository)
        {
            _unitOfWork = unitOfWork;
            _moduleRepository = moduleRepository;
            _operationRepository = operationRepository;
            _loger = loger;
            _loger.Info("Khởi tạo module service");
        }

        public PageListResultBO<ModuleDTO> GetDataByPage(ModuleSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var queryResult = (from module in this._moduleRepository.GetAllAsQueryable()
                               join operation in this._operationRepository.GetAllAsQueryable()
                               on module.Id equals operation.ModuleId
                               into groupOperation
                               select new ModuleDTO()
                               {
                                   Id = module.Id,
                                   Code=module.Code,
                                   Name = module.Name,
                                   IsShow = module.IsShow,
                                   Order = module.Order,
                                   Icon=module.Icon,
                                   ClassCss=module.ClassCss,
                                   StyleCss=module.StyleCss,
                                   OperationQuantity = groupOperation.Count()
                               });
            if (searchParams != null)
            {
                if (!string.IsNullOrEmpty(searchParams.QueryCode))
                {
                    searchParams.QueryCode = searchParams.QueryCode.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Code.Trim().ToLower().Contains(searchParams.QueryCode));
                }
                if (!string.IsNullOrEmpty(searchParams.QueryName))
                {
                    searchParams.QueryName = searchParams.QueryName.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Name.Trim().ToLower().Contains(searchParams.QueryName));
                }
                if (searchParams.QueryIsShow != null)
                {
                    queryResult = queryResult.Where(x => x.IsShow == searchParams.QueryIsShow.Value);
                }
                if (!string.IsNullOrEmpty(searchParams.QueryIcon))
                {
                    searchParams.QueryIcon = searchParams.QueryIcon.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Icon.Trim().ToLower().Contains(searchParams.QueryIcon));
                }
                
                if (!string.IsNullOrEmpty(searchParams.QueryClassCss))
                {
                    searchParams.QueryClassCss = searchParams.QueryClassCss.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.ClassCss.Trim().ToLower().Contains(searchParams.QueryClassCss));
                }
                if (!string.IsNullOrEmpty(searchParams.QueryStyleCss))
                {
                    searchParams.QueryStyleCss = searchParams.QueryStyleCss.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.StyleCss.Trim().ToLower().Contains(searchParams.QueryStyleCss));
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

            var result = new PageListResultBO<ModuleDTO>();
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
        public bool CheckExistCode(string code, long? id = null)
        {
            return _moduleRepository.GetAll().Where(x => x.Code != null && x.Code.Equals(code) && (id.HasValue ? x.Id != id : true)).Any();
        }
    }
}
