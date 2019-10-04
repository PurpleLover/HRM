using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.RoleRepository;
using QLNS.Repository.TD_HoSoUngVienRepository;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using QLNS.Service.Common;
using PagedList;

namespace QLNS.Service.TD_HoSoUngVienService
{
    public class TD_HoSoUngVienService : EntityService<TD_HoSoUngVien>, ITD_HoSoUngVienService
    {
        ITD_HoSoUngVienRepository _HoSoUngVienRepository;
        ILog _iLog;
        IMapper _imapper;

        public TD_HoSoUngVienService(IUnitOfWork unitOfWork, ITD_HoSoUngVienRepository HoSoUngVienRepository, ILog logger, IMapper mapper) :
            base(unitOfWork, HoSoUngVienRepository)
        {
            _HoSoUngVienRepository = HoSoUngVienRepository;
            _iLog = logger;
            _imapper = mapper;
        }

        public PageListResultBO<HoSoUngVienDTO> GetDaTaByPage(HoSoUngVienSearchDTO searchModel, int pageIndex = 1, int pageSize = 20)
        {
            var query = _HoSoUngVienRepository.GetAllAsQueryable();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.sea_HOTEN))
                {
                    searchModel.sea_HOTEN = searchModel.sea_HOTEN.Trim().ToLower();
                    query = query.Where(x => x.HoTen.Trim().ToLower().Contains(searchModel.sea_HOTEN));
                }
                if (!string.IsNullOrEmpty(searchModel.sea_EMAIL))
                {
                    searchModel.sea_EMAIL = searchModel.sea_EMAIL.Trim().ToLower();
                    query = query.Where(x => x.Email.Trim().ToLower().Contains(searchModel.sea_EMAIL));
                }
                if (!string.IsNullOrEmpty(searchModel.sea_DIENTHOAI))
                {
                    searchModel.sea_DIENTHOAI = searchModel.sea_DIENTHOAI.Trim().ToLower();
                    query = query.Where(x => x.DienThoai.Trim().ToLower().Contains(searchModel.sea_DIENTHOAI));
                }
                if (searchModel.sea_GIOITINH != "")
                {
                    query = query.Where(x => x.GioiTinh == searchModel.sea_GIOITINH);
                }
                if (searchModel.sea_NGAYSINH != null)
                {
                    query = query.Where(x => x.NgaySinh == searchModel.sea_NGAYSINH);
                }
                if(searchModel.sea_DOT_ID > 0)
                {
                    query = query.Where(x => x.IDDotTuyenDung == searchModel.sea_DOT_ID);
                }
                if (searchModel.sea_YEUCAU > 0)
                {
                    query = query.Where(x => x.IdYeuCauTuyenDung == searchModel.sea_YEUCAU);
                }
                if (!string.IsNullOrEmpty(searchModel.sortQuery))
                {
                    query = query.OrderBy(searchModel.sortQuery);
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
            var resultmodel = new PageListResultBO<TD_HoSoUngVien>();
            if (pageSize == -1)
            {
                var dataPageList = query.ToList();
                resultmodel.Count = dataPageList.Count;
                resultmodel.TotalPage = 1;
                resultmodel.ListItem = dataPageList;
            }
            else
            {
                var dataPageList = query.ToPagedList(pageIndex, pageSize);
                resultmodel.Count = dataPageList.TotalItemCount;
                resultmodel.TotalPage = dataPageList.PageCount;
                resultmodel.ListItem = dataPageList.ToList();
            }

            return _imapper.Map<PageListResultBO<HoSoUngVienDTO>>(resultmodel);
        }
        public HoSoUngVienDTO GetInfoById(int id)
        {
            var obj = _HoSoUngVienRepository.GetById(id);
            
            var result = _imapper.Map<HoSoUngVienDTO>(obj);
            return result;
        }

        public List<HoSoUngVienDTO> GetAllDataInfo()
        {
            var obj = _HoSoUngVienRepository.GetAll();

            var result = _imapper.Map<List<HoSoUngVienDTO>>(obj);
            return result;
        }
        public List<HoSoUngVienDTO> GetHoSoOfDotTuyenDung(int dotTuyenDungId)
        {
            var obj = _HoSoUngVienRepository.GetAllAsQueryable().Where(x=>x.IDDotTuyenDung== dotTuyenDungId).ToList();

            var result = _imapper.Map<List<HoSoUngVienDTO>>(obj);
            return result;
        }
    }
}
