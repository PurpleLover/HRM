using AutoMapper;
using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.TD_DotTuyenDungRepository;
using QLNS.Repository.TD_YeuCauVaDotTuyenDungRepository;
using QLNS.Repository.RecruitmentRequestRepository;

using QLNS.Service.Common;
using QLNS.Service.TD_DotTuyenDungService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using QLNS.Repository.RecruitmentRequestRespository;
using System.Web.Mvc;
using QLNS.Service.RecruitmentRequestService.DTO;
using CommonHelper.ObjectExtention;
using QLNS.Service.Constant;
using QLNS.Service.RecruitmentRequestService.DTO;

namespace QLNS.Service.TD_DotTuyenDungService
{
    public class TD_DotTuyenDungService : EntityService<TD_DotTuyenDung>, ITD_DotTuyenDungService
    {
        ITD_DotTuyenDungRepository _DotTuyenDungRepository;
        ITD_YeuCauVaDotTuyenDungRepository _YeuCauVaDotTuyenDungRepository;
        IRecruitmentRequestRepository _RecruitmentRequestRepository;
        ILog _iLog;
        IMapper _imapper;
        IUnitOfWork _unitOfWork;

        public TD_DotTuyenDungService(IUnitOfWork unitOfWork, ITD_DotTuyenDungRepository DotTuyenDungRepository,
            ITD_YeuCauVaDotTuyenDungRepository YeuCauVaDotTuyenDungRepository,
            IRecruitmentRequestRepository RecruitmentRequestRepository,
            ILog logger, IMapper mapper) :
            base(unitOfWork, DotTuyenDungRepository)
        {
            _unitOfWork = unitOfWork;
            _YeuCauVaDotTuyenDungRepository = YeuCauVaDotTuyenDungRepository;
            _RecruitmentRequestRepository = RecruitmentRequestRepository;
            _DotTuyenDungRepository = DotTuyenDungRepository;
            _iLog = logger;
            _imapper = mapper;
        }

        public PageListResultBO<DotTuyenDungDTO> GetDaTaByPage(DotTuyenDungSearchDTO searchModel, int pageIndex = 1, int pageSize = 20)
        {
            var query = _DotTuyenDungRepository.GetAllAsQueryable();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.seaTenDot))
                {
                    searchModel.seaTenDot = searchModel.seaTenDot.Trim().ToLower();
                    query = query.Where(x => x.TenDot.Trim().ToLower().Contains(searchModel.seaTenDot));
                }
                if (searchModel.seaTrangThai != "")
                {
                    query = query.Where(x => x.TrangThai == searchModel.seaTrangThai);
                }
                if (searchModel.ngaybatdaufrom != null)
                {
                    query = query.Where(x => x.NgayBatDau >= searchModel.ngaybatdaufrom);
                }
                if (searchModel.ngaybatdauto != null)
                {
                    query = query.Where(x => x.NgayBatDau <= searchModel.ngaybatdauto);
                }
                if (searchModel.ngayketthucfrom != null)
                {
                    query = query.Where(x => x.NgayKetThuc >= searchModel.ngayketthucfrom);
                }
                if (searchModel.ngayketthucto != null)
                {
                    query = query.Where(x => x.NgayKetThuc <= searchModel.ngayketthucto);
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
            var resultmodel = new PageListResultBO<TD_DotTuyenDung>();
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
            return _imapper.Map<PageListResultBO<DotTuyenDungDTO>>(resultmodel);
        }

        /// <summary>
        /// Lấy danh sách yêu cầu tuyển dụng của đợt tuyển dụng
        /// </summary>
        /// <param name="dotTuyenDungID"></param>
        /// <returns></returns>
        public List<RecruitmentRequestDTO> GetYeuCauCuaDotTuyenDung(int dotTuyenDungID)
        {
            var lstRecruitmentRequest = (from ycdtd in _YeuCauVaDotTuyenDungRepository.GetAllAsQueryable().Where(x => x.IdDotTuyenDung == dotTuyenDungID)
                                         join yc in _RecruitmentRequestRepository.GetAllAsQueryable() on ycdtd.IdYeuCauTuyenDung equals yc.Id
                                         select yc).ToList();

            return _imapper.Map<List<RecruitmentRequestDTO>>(lstRecruitmentRequest);
        }

        public List<SelectListItem> GetYeuCauCuaDotTuyenDungDropdownlist(int dotTuyenDungID, int? selectedId = null)
        {
            var lstRecruitmentRequest = (from ycdtd in _YeuCauVaDotTuyenDungRepository.GetAllAsQueryable().Where(x => x.IdDotTuyenDung == dotTuyenDungID)
                                         join yc in _RecruitmentRequestRepository.GetAllAsQueryable() on ycdtd.IdYeuCauTuyenDung equals yc.Id
                                         select new SelectListItem()
                                         {
                                             Text = yc.Title,
                                             Value = yc.Id.ToString(),
                                             Selected = selectedId.HasValue && selectedId == yc.Id ? true : false
                                         }).ToList();

            return lstRecruitmentRequest;
        }

        /// <summary>
        /// Lưu yêu cầu tuyển dụng của đợt tuyển dụng
        /// </summary>
        /// <param name="dsYeuCauId"></param>
        /// <param name="dotTuyenDungId"></param>
        /// <returns></returns>
        public bool SaveYeuCauOfDotTuyenDung(List<int> dsYeuCauId, int dotTuyenDungId)
        {
            try
            {


                var lstYeuCau = _YeuCauVaDotTuyenDungRepository.GetAllAsQueryable().Where(x => x.IdDotTuyenDung == dotTuyenDungId).ToList();
                var diff = CompareListDataSave.GetDifferent<int>(dsYeuCauId, lstYeuCau.Select(x => x.IdYeuCauTuyenDung).ToList());
                //Thêm mới
                if (diff.Item1 != null && diff.Item1.Any())
                {
                    foreach (var newData in diff.Item1)
                    {
                        var yeucau = _RecruitmentRequestRepository.GetById(newData);
                        if (yeucau != null)
                        {
                            var objYeuCauDotTuyenDung = new TD_YeuCauVaDotTuyenDung()
                            {
                                IdDotTuyenDung = dotTuyenDungId,
                                IdYeuCauTuyenDung = newData
                            };
                            _YeuCauVaDotTuyenDungRepository.Add(objYeuCauDotTuyenDung);
                            yeucau.Status = YeuCauTuyenDungTrangThaiConst.DangTuyen;
                            _RecruitmentRequestRepository.Edit(yeucau);
                        }
                    }
                }
                //Xoa yeu cau cu
                if (diff.Item2 != null && diff.Item2.Any())
                {
                    foreach (var oldData in diff.Item2)
                    {
                        var yeucau = _RecruitmentRequestRepository.GetById(oldData);
                        if (yeucau != null)
                        {
                            var objYeuCauDotTuyenDung = lstYeuCau.Where(x => x.IdYeuCauTuyenDung == oldData).ToList();
                            if (objYeuCauDotTuyenDung != null)
                            {
                                _YeuCauVaDotTuyenDungRepository.DeleteRange(objYeuCauDotTuyenDung);
                            }
                            yeucau.Status = YeuCauTuyenDungTrangThaiConst.MoiTao;
                            _RecruitmentRequestRepository.Edit(yeucau);
                        }
                    }
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _iLog.Error("Lỗi khi lưu yêu cầu tuyển dụng cho đợt tuyển dụng", ex);
                return false;
            }
        }

        public List<SelectListItem> DropdownListDotTuyenDung(int? selected = 0)
        {
            var query = _DotTuyenDungRepository.GetAllAsQueryable().Where(x => x.TrangThai == DotTuyenDungTrangThaiConst.DaDuyet).Select(x => new SelectListItem()
            {
                Text = x.TenDot,
                Value = x.Id.ToString(),
                Selected = (x.Id == selected)
            }).ToList();
            return query;
        }
    }
}
