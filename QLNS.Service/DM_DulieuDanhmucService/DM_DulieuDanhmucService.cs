using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.DanhmucRepository;
using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QLNS.Service.DM_DulieuDanhmucService
{
    public class DM_DulieuDanhmucService : EntityService<DM_DulieuDanhmuc>, IDM_DulieuDanhmucService
    {
        IUnitOfWork _unitOfWork;
        IDM_DulieuDanhmucRepository _dM_DulieuDanhmucRepository;
        IDM_NhomDanhmucRepository _nhomDanhmucRepository;
        ILog _loger;

        public DM_DulieuDanhmucService(IUnitOfWork unitOfWork, IDM_DulieuDanhmucRepository dM_DulieuDanhmucRepository, IDM_NhomDanhmucRepository nhomDanhmucRepository, ILog loger) : base(unitOfWork, dM_DulieuDanhmucRepository)
        {
            _unitOfWork = unitOfWork;
            _dM_DulieuDanhmucRepository = dM_DulieuDanhmucRepository;
            _nhomDanhmucRepository = nhomDanhmucRepository;
            _loger = loger;
            _loger.Info("Khởi tạo DM_DulieuDanhmuc service");
        }

        /// <summary>
        /// Lấy danh sách theo tên
        /// </summary>
        /// <param name="GroupCode"></param>
        /// <returns></returns>
        public List<DM_DulieuDanhmuc> GetByCodeGroup(string GroupCode)
        {
            var group = _nhomDanhmucRepository.FindBy(x => x.GroupCode.Equals(GroupCode)).FirstOrDefault();
            if (group == null)
            {
                return new List<DM_DulieuDanhmuc>();
            }
            var listData = _dM_DulieuDanhmucRepository.GetAllAsQueryable().Where(x => x.GroupId == group.Id).ToList();
            return listData;
        }

        /// <summary>
        /// Lấy danh sách dropdown
        /// </summary>
        /// <param name="GroupCode"></param>
        /// <returns></returns>
        public List<SelectListItem> GetDropdownlist(string GroupCode, string SelectedValue)
        {
            var group = _nhomDanhmucRepository.FindBy(x => x.GroupCode.Equals(GroupCode)).FirstOrDefault();
            if (group == null)
            {
                return new List<SelectListItem>();
            }
            var listData = _dM_DulieuDanhmucRepository.GetAllAsQueryable().Where(x => x.GroupId == group.Id).
                Select(x => new SelectListItem()
                {
                    Text=x.Name,
                    Value=x.Code,
                    Selected= SelectedValue== x.Code
                }).ToList();

            return listData;
        }
        public PageListResultBO<DM_DulieuDanhmucDTO> GetDataByPage(long danhMucId, DM_DulieuDanhmucSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var query = (from dulieuDanhmuc in this._dM_DulieuDanhmucRepository.GetAllAsQueryable()
                         where dulieuDanhmuc.GroupId == danhMucId
                         select new DM_DulieuDanhmucDTO()
                         {
                             Id = dulieuDanhmuc.Id,
                             Code = dulieuDanhmuc.Code,
                             Name = dulieuDanhmuc.Name,
                             Note = dulieuDanhmuc.Note,
                             Priority = dulieuDanhmuc.Priority
                         });
            if (searchParams != null)
            {
                if (!string.IsNullOrEmpty(searchParams.sortQuery))
                {
                    query = query.OrderBy(searchParams.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                if (!String.IsNullOrEmpty(searchParams.QueryCode))
                {
                    query = query.Where(x => x.Code.Contains(searchParams.QueryCode));
                }
                if (!String.IsNullOrEmpty(searchParams.QueryName))
                {
                    query = query.Where(x => x.Name.Contains(searchParams.QueryName));
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }

            var result = new PageListResultBO<DM_DulieuDanhmucDTO>();
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

        public List<DM_DulieuDanhmuc> GetListDataByGroupId(long groupId)
        {
            return this._dM_DulieuDanhmucRepository.GetAllAsQueryable().Where(x => x.GroupId == groupId).ToList();
        }

        public bool CheckCodeExisted(long? groupId, string code)
        {
            return this._dM_DulieuDanhmucRepository.GetAllAsQueryable().Where(x => x.GroupId == groupId && x.Code.Equals(code)).Any();
        }
    }
}
