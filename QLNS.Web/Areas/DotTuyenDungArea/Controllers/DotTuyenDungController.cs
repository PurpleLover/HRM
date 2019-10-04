using AutoMapper;
using CommonHelper.ObjectExtention;
using CommonHelper.String;
using log4net;
using Newtonsoft.Json;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using QLNS.Service.RecruitmentRequestService;
using QLNS.Service.TaiLieuDinhKemService;
using QLNS.Service.TD_DotTuyenDungService;
using QLNS.Service.TD_DotTuyenDungService.DTO;
using QLNS.Service.TD_HoSoUngVienService;
using QLNS.Web.Areas.DotTuyenDungArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace QLNS.Web.Areas.DotTuyenDungArea.Controllers
{
    public class DotTuyenDungController : BaseController
    {
        private ILog _logger;
        private IMapper _mapper;
        private readonly ITD_DotTuyenDungService _DotTuyenDungService;
        private readonly IRecruitmentRequestService _RecruitmentRequestService;
        private readonly ITaiLieuDinhKemService _taiLieuDinhKemService;
        private readonly ITD_HoSoUngVienService _HoSoUngVienService;
        private string KeySearchModelSession = "DotTuyenDungSearchModel";

        // GET: DotTuyenDungArea/DotTuyenDung

        public DotTuyenDungController(ITD_DotTuyenDungService DotTuyenDungService,
            ITaiLieuDinhKemService taiLieuDinhKemService,
            IRecruitmentRequestService RecruitmentRequestService,
            ITD_HoSoUngVienService HoSoUngVienService,
            ILog logger, IMapper mapper)
        {
            _HoSoUngVienService = HoSoUngVienService;
            _RecruitmentRequestService = RecruitmentRequestService;
            _taiLieuDinhKemService = taiLieuDinhKemService;
            _DotTuyenDungService = DotTuyenDungService;
            _logger = logger;
            _mapper = mapper;
        }
        public ActionResult Index()
        {
            var model = _DotTuyenDungService.GetDaTaByPage(null);
            SessionManager.SetValue(KeySearchModelSession, null);
            return View(model);
        }
        [HttpPost]
        public JsonResult getData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(KeySearchModelSession) as DotTuyenDungSearchDTO;
            if (!string.IsNullOrEmpty(sortQuery))
            {
                if (searchModel == null)
                {
                    searchModel = new DotTuyenDungSearchDTO();
                }
                searchModel.sortQuery = sortQuery;
                if (pageSize > 0)
                {
                    searchModel.pageSize = pageSize;
                }
                SessionManager.SetValue(KeySearchModelSession, searchModel);
            }
            var data = _DotTuyenDungService.GetDaTaByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult searchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(KeySearchModelSession) as DotTuyenDungSearchDTO;

            if (searchModel == null)
            {
                searchModel = new DotTuyenDungSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.seaTenDot = form["seaTenDot"];
            searchModel.seaTrangThai = form["seaTrangThai"];
            searchModel.ngaybatdaufrom = form["ngaybatdaufrom"].ToDateTime();
            searchModel.ngaybatdauto = form["ngaybatdauto"].ToDateTime();
            searchModel.ngayketthucfrom = form["ngayketthucfrom"].ToDateTime();
            searchModel.ngayketthucto = form["ngayketthucto"].ToDateTime();

            SessionManager.SetValue(KeySearchModelSession, searchModel);

            var data = _DotTuyenDungService.GetDaTaByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var listYeuCauTuyenDung = _RecruitmentRequestService.GetRecruitmentRequestsNew();
            ViewBag.listYeuCauTuyenDung = listYeuCauTuyenDung;
            var model = new CreateDotTuyenDungModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(CreateDotTuyenDungModel model, List<int> DsYeuCauTuyenDung, List<string> name_FileDotTuyenDung, List<HttpPostedFileBase> FileDotTuyenDung)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _mapper.Map<TD_DotTuyenDung>(model);
                    EntityModel.TrangThai = DotTuyenDungTrangThaiConst.MoiTao;
                    _DotTuyenDungService.Create(EntityModel);
                    _DotTuyenDungService.SaveYeuCauOfDotTuyenDung(DsYeuCauTuyenDung, EntityModel.Id);

                    if (FileDotTuyenDung != null && FileDotTuyenDung.Any())
                    {
                        var resultfield = _taiLieuDinhKemService.SaveMultiFile(LoaiTaiLieuUploadConstant.DotTuyenDung, EntityModel.Id, FileDotTuyenDung, name_FileDotTuyenDung, null, null, LoaiTaiLieuUploadConstant.DotTuyenDung, HostingEnvironment.MapPath("/Uploads"), CurrentUserId);
                        if (!resultfield.Status)
                        {
                            TempData["MessageError"] = resultfield.Message;
                        }
                    }
                    TempData["MessageSuccess"] = "Tạo đợt tuyển dụng thành công";
                    return RedirectToAction("Index");
                }
                TempData["MessageError"] = "Lỗi dữ liệu";

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                _logger.Error("Lỗi khi lưu thông tin đợt tuyển dụng", ex);
                throw new HttpException("Lỗi khi lưu dữ liệu đợt tuyển dụng", ex);
            }

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objDB = _DotTuyenDungService.GetById(id);
            if (objDB == null)
            {
                return HttpNotFound();
            }
            var listYeuCauTuyenDung = _RecruitmentRequestService.GetRecruitmentRequestsNew();
            ViewBag.listYeuCauTuyenDung = listYeuCauTuyenDung;

            var model = _mapper.Map<TD_DotTuyenDung, EditDotTuyenDungModel>(objDB);
            var listYeuCauTuyenDungDot = _DotTuyenDungService.GetYeuCauCuaDotTuyenDung(objDB.Id);

            ViewBag.listYeuCauTuyenDungDot = listYeuCauTuyenDungDot;
            var tailieudinhkem = _taiLieuDinhKemService.GetListTaiLieuAllByType(LoaiTaiLieuUploadConstant.DotTuyenDung, objDB.Id);
            ViewBag.tailieudinhkem = tailieudinhkem;

            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeStatus(long id, string status)
        {
            var result = new JsonResultBO(true, "Thay đổi trạng thái thành công");
            var obj = _DotTuyenDungService.GetById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            obj.TrangThai = status;
            _DotTuyenDungService.Update(obj);
            return Json(result);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveEdit(EditDotTuyenDungModel model, List<int> DsYeuCauTuyenDung, List<string> name_FileDotTuyenDung, List<HttpPostedFileBase> FileDotTuyenDung)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _DotTuyenDungService.GetById(model.Id);
                    EntityModel = _mapper.Map(model, EntityModel);
                    _DotTuyenDungService.Update(EntityModel);
                    _DotTuyenDungService.SaveYeuCauOfDotTuyenDung(DsYeuCauTuyenDung, EntityModel.Id);

                    if (FileDotTuyenDung != null && FileDotTuyenDung.Any())
                    {
                        var resultfield = _taiLieuDinhKemService.SaveMultiFile(LoaiTaiLieuUploadConstant.DotTuyenDung, EntityModel.Id, FileDotTuyenDung, name_FileDotTuyenDung, null, null, LoaiTaiLieuUploadConstant.DotTuyenDung, HostingEnvironment.MapPath("/Uploads"), CurrentUserId);
                        if (!resultfield.Status)
                        {
                            TempData["MessageError"] = resultfield.Message;

                        }
                    }
                    TempData["MessageSuccess"] = "Cập nhật thông tin tuyển dụng thành công";
                    return RedirectToAction("Index");
                }
                TempData["MessageError"] = "Lỗi dữ liệu";

                return RedirectToAction("Edit", new { model.Id });
            }
            catch (Exception ex)
            {
                _logger.Error("Lỗi khi lưu thông tin đợt tuyển dụng", ex);
                throw new HttpException("Lỗi khi lưu dữ liệu đợt tuyển dụng", ex);
            }

        }

        public ActionResult Delete(long id)
        {
            var obj = _DotTuyenDungService.GetById(id);
            if (obj == null)
            {
                throw new HttpException(404, "Không tìm thấy dữ liệu");
            }
            var result = new JsonResultBO(true, "Xóa đợt tuyển dụng thành công");
            _DotTuyenDungService.Delete(obj);
            _logger.InfoFormat("Xóa đợt tuyển dụng: {0}", JsonConvert.SerializeObject(obj));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(long id)
        {
            var obj = _DotTuyenDungService.GetById(id);
            if (obj == null)
            {
                throw new HttpException(404, "Không tìm thấy dữ liệu");
            }
            var model = new DetailViewModel();
            model.taiLieuDinhKem = _taiLieuDinhKemService.GetListTaiLieuAllByType(LoaiTaiLieuUploadConstant.DotTuyenDung, obj.Id);
            model.listYeuCauTuyenDungDot = _DotTuyenDungService.GetYeuCauCuaDotTuyenDung(obj.Id);
            model.dotTuyenDung = obj;
            model.dsUngVien = _HoSoUngVienService.GetHoSoOfDotTuyenDung(obj.Id);
            return View(model);
        }

        public JsonResult GetDsYeuCauOfDot(int? id)
        {
            var result = new List<SelectListItem>();
            if (!id.HasValue)
            {
                return Json(result);
            }
            result = _DotTuyenDungService.GetYeuCauCuaDotTuyenDungDropdownlist(id.Value);
            return Json(result);
        }
    }
}