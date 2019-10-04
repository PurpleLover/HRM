using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.CommonConfigurationRepository;
using QLNS.Service.Common;
using QLNS.Service.CommonConfigurationService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.CommonConfigurationService
{
    public class CommonConfigurationService : EntityService<CommonConfiguration>, ICommonConfigurationService
    {
        IUnitOfWork _unitOfWork;
        ICommonConfigurationRepository _commonConfigurationRepository;
        ILog _loger;

        public CommonConfigurationService(IUnitOfWork unitOfWork, ICommonConfigurationRepository commonConfigurationRepository, ILog loger) : base(unitOfWork, commonConfigurationRepository)
        {
            _unitOfWork = unitOfWork;
            _commonConfigurationRepository = commonConfigurationRepository;
            _loger = loger;
            _loger.Info("Khởi tạo CommonConfiguration service");
        }
        public PageListResultBO<CommonConfigurationDTO> GetDataByPage(CommonConfigurationSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var query = (from tbl in this._commonConfigurationRepository.GetAllAsQueryable()
                         select new CommonConfigurationDTO()
                         {
                             Id = tbl.Id,
                             ConfigCode = tbl.ConfigCode,
                             ConfigName = tbl.ConfigName,
                             ConfigData = tbl.ConfigData
                         });
            if (searchParams != null)
            {
                if (!String.IsNullOrEmpty(searchParams.QueryCode))
                {
                    query = query.Where(x => x.ConfigCode.Contains(searchParams.QueryCode));
                }
                if (!String.IsNullOrEmpty(searchParams.QueryName))
                {
                    query = query.Where(x => x.ConfigName.Contains(searchParams.QueryName));
                }

                if (!string.IsNullOrEmpty(searchParams.sortQuery))
                {
                    query = query.OrderBy(searchParams.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }

            var result = new PageListResultBO<CommonConfigurationDTO>();
            if (pageSize == -1)
            {
                var pagedList = query.ToList();
                result.Count = pagedList.Count;
                result.TotalPage = 1;
                result.ListItem = pagedList;
            }
            else
            {
                var dataPageList = query.ToPagedList(pageIndex, pageSize);
                result.Count = dataPageList.TotalItemCount;
                result.TotalPage = dataPageList.PageCount;
                result.ListItem = dataPageList.ToList();
            }
            return result;
        }
        
        public string GetDataByCode(string code)
        {
            return this._commonConfigurationRepository.GetAllAsQueryable().Where(x => x.ConfigCode.Equals(code)).Select(x=>x.ConfigData).FirstOrDefault();
        }

        public bool CheckCodeExisted(string code)
        {
            return this._commonConfigurationRepository.GetAllAsQueryable().Where(x => x.ConfigCode.Equals(code)).Any();
        }
    }
}
