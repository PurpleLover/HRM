using AutoMapper;
using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.DanhmucRepository;
using QLNS.Service.Common;
using QLNS.Service.DM_NhomDanhmucService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.DM_NhomDanhmucService
{
    public class DM_NhomDanhmucService : EntityService<DM_NhomDanhmuc>, IDM_NhomDanhmucService
    {
        IUnitOfWork _unitOfWork;
        IDM_NhomDanhmucRepository _dM_NhomDanhmucRepository;
        IDM_DulieuDanhmucRepository categoryDataRepository;
        ILog _loger;
        IMapper mapper;
        public DM_NhomDanhmucService(IUnitOfWork unitOfWork,
            IDM_NhomDanhmucRepository dmNhomDanhmucRepository,
            IDM_DulieuDanhmucRepository categoryDataRepository,
            IMapper mapper,
        ILog loger) : base(unitOfWork, dmNhomDanhmucRepository)
        {
            _unitOfWork = unitOfWork;
            _dM_NhomDanhmucRepository = dmNhomDanhmucRepository;
            this.categoryDataRepository = categoryDataRepository;
            this.mapper = mapper;
            _loger = loger;
            _loger.Info("Khởi tạo DM_NhomDanhmuc service");
        }

        public PageListResultBO<DM_NhomDanhmucDTO> GetDataByPage(DM_NhomDanhmucSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var query = (from nhomDanhmuc in this._dM_NhomDanhmucRepository.GetAllAsQueryable()
                         select new DM_NhomDanhmucDTO()
                         {
                             Id = nhomDanhmuc.Id,
                             GroupCode = nhomDanhmuc.GroupCode,
                             GroupName = nhomDanhmuc.GroupName
                         });
            if (searchParams != null)
            {
                if (!String.IsNullOrEmpty(searchParams.QueryCode))
                {
                    query = query.Where(x => x.GroupCode.Contains(searchParams.QueryCode));
                }
                if (!String.IsNullOrEmpty(searchParams.QueryName))
                {
                    query = query.Where(x => x.GroupName.Contains(searchParams.QueryName));
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

            var result = new PageListResultBO<DM_NhomDanhmucDTO>();
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

        public bool CheckGroupCodeExisted(string groupCode)
        {
            return this._dM_NhomDanhmucRepository.GetAllAsQueryable().Where(x => x.GroupCode.Equals(groupCode)).Any();
        }

        /// <summary>
        /// @author:duynn
        /// @description: lấy danh mục bằng nhóm
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetDataByCode(string code)
        {
            var result = (from data in this.categoryDataRepository.GetAllAsQueryable()
                          join groupData in this._dM_NhomDanhmucRepository.GetAllAsQueryable()
                          on data.GroupId equals groupData.Id
                          where groupData.GroupCode == code
                          select data).AsEnumerable()
                          .Select(data => mapper.Map<DM_DulieuDanhmuc, SelectListItem>(data)).ToList();
            return result;
        }
    }
}
