using log4net;
using QLNS.Service.Constant;
using QLNS.Service.DM_NhomDanhmucService;
using QLNS.Service.DM_DulieuDanhmucService;
using QLNS.Service.TD_HoSoUngVienService;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using QLNS.Web.Areas.TDHoSoUngVienArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using QLNS.Model.Entities;
using QLNS.Service.TaiLieuDinhKemService;
using System.Web.Hosting;
using System.Threading;
using Microsoft.AspNet.Identity;
using QLNS.Service.Common;
using Newtonsoft.Json;
using CommonHelper.String;
using QLNS.Service.TD_DotTuyenDungService;

namespace QLNS.Web.Areas.TDHoSoUngVienArea.Controllers
{
    public class TDHoSoUngVienController : BaseController
    {
        // GET: TDHoSoUngVienArea/TDHoSoUngVien
        private readonly ITD_HoSoUngVienService _hoSoUngVienService;
        private readonly IDM_NhomDanhmucService _NhomDanhMucService;
        private readonly IDM_DulieuDanhmucService _DulieuDanhmucService;
        private readonly ITaiLieuDinhKemService _taiLieuDinhKemService;
        private readonly ITD_DotTuyenDungService _DotTuyenDungService;
        private readonly ILog _loger;
        private readonly IMapper _mapper;
        private string KeySearchModelSession = "TDHoSoUngVienSearchModel";
        public TDHoSoUngVienController(
            ITD_HoSoUngVienService hoSoUngVienService,
            IDM_NhomDanhmucService NhomDanhMucService,
            IDM_DulieuDanhmucService DulieuDanhmucService,
            ITaiLieuDinhKemService taiLieuDinhKemService,
            ITD_DotTuyenDungService DotTuyenDungService,
            IMapper mapper,
            ILog loger
            )
        {
            _DotTuyenDungService = DotTuyenDungService;
            _taiLieuDinhKemService = taiLieuDinhKemService;
            _NhomDanhMucService = NhomDanhMucService;
            _hoSoUngVienService = hoSoUngVienService;
            _DulieuDanhmucService = DulieuDanhmucService;
            _loger = loger;
            _mapper = mapper;
        }
        [Authorize]
        public ActionResult Index()
        {
            var userData = _hoSoUngVienService.GetDaTaByPage(null);
            SessionManager.SetValue(KeySearchModelSession, null);


            return View(userData);
        }

        [HttpPost]
        public JsonResult getData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(KeySearchModelSession) as HoSoUngVienSearchDTO;
            if (!string.IsNullOrEmpty(sortQuery))
            {
                if (searchModel == null)
                {
                    searchModel = new HoSoUngVienSearchDTO();
                }
                searchModel.sortQuery = sortQuery;
                if (pageSize > 0)
                {
                    searchModel.pageSize = pageSize;
                }
                SessionManager.SetValue(KeySearchModelSession, searchModel);
            }
            var data = _hoSoUngVienService.GetDaTaByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult searchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(KeySearchModelSession) as HoSoUngVienSearchDTO;

            if (searchModel == null)
            {
                searchModel = new HoSoUngVienSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.sea_HOTEN = form["sea_HOTEN"];
            searchModel.sea_EMAIL = form["sea_EMAIL"];
            searchModel.sea_DIENTHOAI = form["sea_DIENTHOAI"];
            searchModel.sea_GIOITINH = form["sea_GIOITINH"];
            searchModel.sea_NGAYSINH = form["sea_NGAYSINH"].ToDateTime();
            searchModel.sea_YEUCAU = form["sea_YEUCAU"].ToIntOrZero();
            searchModel.sea_DOT_ID = form["sea_DOT_ID"].ToIntOrZero();
            SessionManager.SetValue(KeySearchModelSession, searchModel);

            var data = _hoSoUngVienService.GetDaTaByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DanTocDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.DanToc,string.Empty).AddDefault("--Chọn dân tộc--");
            ViewBag.TonGiaoDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.TonGiao, string.Empty).AddDefault("--Chọn tôn giáo--");
            ViewBag.QuocGiaDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.QuocGia, string.Empty).AddDefault("--Chọn quốc tịch--");
            ViewBag.XuatThanDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.XuatThan, string.Empty).AddDefault("--Chọn xuất thân--");
            ViewBag.KenhUngTuyenDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.KenhUngTuyen, string.Empty).AddDefault("--Chọn kênh ứng tuyển--");
            ViewBag.DotTuyenDung = _DotTuyenDungService.DropdownListDotTuyenDung().AddDefault("--Chọn đợt tuyển dụng--");

            var model = new CreateHoSoUngVienModel();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveCreate(CreateHoSoUngVienModel model, HttpPostedFileBase AvatarUser, List<string> name_FileHoSoUngVien, List<HttpPostedFileBase> FileHoSoUngVien)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _mapper.Map<TD_HoSoUngVien>(model);
                    _hoSoUngVienService.Create(EntityModel);
                    if (FileHoSoUngVien != null && FileHoSoUngVien.Any())
                    {
                        var resultfield = _taiLieuDinhKemService.SaveMultiFile(LoaiTaiLieuUploadConstant.HoSoUngVien, EntityModel.Id, FileHoSoUngVien, name_FileHoSoUngVien, null, null, LoaiTaiLieuUploadConstant.HoSoUngVien, HostingEnvironment.MapPath("/Uploads"), CurrentUserId);
                        if (!resultfield.Status)
                        {
                            TempData["MessageError"] = resultfield.Message;

                        }
                    }
                    TempData["MessageSuccess"] = "Tạo hồ sơ ứng viên thành công";
                    return RedirectToAction("Index");
                }
                TempData["MessageError"] = "Lỗi dữ liệu";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                _loger.Error("Lỗi khi lưu thông tin ứng viên", ex);
                throw new HttpException("Lỗi khi lưu dữ liệu hồ sơ ứng viên", ex);
            }

        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var modelDB = _hoSoUngVienService.GetById(id);
            if (modelDB == null)
            {
                return HttpNotFound();
            }
            var model = _mapper.Map<TD_HoSoUngVien, EditHoSoUngVienModel>(modelDB);
            ViewBag.DanTocDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.DanToc, model.DanToc).AddDefault("--Chọn dân tộc--");
            ViewBag.TonGiaoDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.TonGiao, model.TonGiao).AddDefault("--Chọn tôn giáo--");
            ViewBag.QuocGiaDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.QuocGia, model.QuocTich).AddDefault("--Chọn quốc tịch--");
            ViewBag.XuatThanDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.XuatThan, model.XuatThan).AddDefault("--Chọn xuất thân--");
            ViewBag.KenhUngTuyenDropdownData = _DulieuDanhmucService.GetDropdownlist(DanhMucConstant.KenhUngTuyen, model.KenhUngTuyen).AddDefault("--Chọn kênh ứng tuyển--");
            ViewBag.DotTuyenDung = _DotTuyenDungService.DropdownListDotTuyenDung().AddDefault("--Chọn đợt tuyển dụng--");
            ViewBag.YeuCauTuyenDung = _DotTuyenDungService.GetYeuCauCuaDotTuyenDungDropdownlist(modelDB.IDDotTuyenDung.GetValueOrDefault(),model.IdYeuCauTuyenDung).AddDefault("--Chọn đợt tuyển dụng--");

            ViewBag.lstTaiLieu = _taiLieuDinhKemService.GetListTaiLieuAllByType(LoaiTaiLieuUploadConstant.HoSoUngVien, model.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(EditHoSoUngVienModel model, HttpPostedFileBase AvatarUser, List<string> name_FileHoSoUngVien, List<HttpPostedFileBase> FileHoSoUngVien)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EntityModel = _hoSoUngVienService.GetById(model.Id);
                    EntityModel = _mapper.Map(model, EntityModel);
                    _hoSoUngVienService.Update(EntityModel);
                    if (FileHoSoUngVien != null && FileHoSoUngVien.Any())
                    {
                        var resultfield = _taiLieuDinhKemService.SaveMultiFile(LoaiTaiLieuUploadConstant.HoSoUngVien, EntityModel.Id, FileHoSoUngVien, name_FileHoSoUngVien, null, null, LoaiTaiLieuUploadConstant.HoSoUngVien, HostingEnvironment.MapPath("/Uploads"), CurrentUserId);
                        if (!resultfield.Status)
                        {
                            TempData["MessageError"] = resultfield.Message;

                        }
                    }
                    TempData["MessageSuccess"] = "Cập nhật thành công hồ sơ ứng viên thành công";
                    return RedirectToAction("Index");
                }
                TempData["MessageError"] = "Lỗi dữ liệu";

                return RedirectToAction("Edit", new { id = model.Id });
            }
            catch (Exception ex)
            {
                _loger.Error("Lỗi khi lưu thông tin ứng viên", ex);
                throw new HttpException("Lỗi khi lưu dữ liệu hồ sơ ứng viên", ex);
            }

        }

        public ActionResult Delete(long id)
        {
            var obj = _hoSoUngVienService.GetById(id);
            if (obj == null)
            {
                throw new HttpException(404, "Không tìm thấy dữ liệu");
            }
            var result = new JsonResultBO(true, "Xóa hồ sơ ứng viên thành công");
            _hoSoUngVienService.Delete(obj);
            _loger.InfoFormat("Xóa hồ sơ ứng viên: {0}", JsonConvert.SerializeObject(obj));
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(int id)
        {
            var model = new DetailModel();
            model.hoSoUngVien = _hoSoUngVienService.GetById(id);
            return View(model);
        }

    }
}