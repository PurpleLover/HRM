using AutoMapper;
using log4net;
using Newtonsoft.Json;
using QLNS.Model.Common;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService;
using QLNS.Service.TD_CauHinhSangLocUngVienService;
using QLNS.Service.TD_QuanLyMauTestService;
using QLNS.Web.Areas.TdCauHinhSangLocUngVienArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.TdCauHinhSangLocUngVienArea.Controllers
{
    public class TdCauHinhSangLocUngVienController : BaseController
    {
        // GET: TdCauHinhSangLocUngVienArea/TdCauHinhSangLocUngVien
        ITD_CauHinhSangLocUngVienService _TD_CauHinhSangLocUngVienService;
        ITD_QuanLyMauTestService _quanLyMauTestService;
        IDM_DulieuDanhmucService _dmDulieuDanhmucService;
        ILog _ILog;
        IMapper _IMapper;

        public TdCauHinhSangLocUngVienController(ITD_CauHinhSangLocUngVienService TD_CauHinhSangLocUngVienService,
            IDM_DulieuDanhmucService dmDulieuDanhmucService,
            ITD_QuanLyMauTestService quanLyMauTestService,
            ILog ILog, IMapper IMapper)
        {
            _TD_CauHinhSangLocUngVienService = TD_CauHinhSangLocUngVienService;
            _dmDulieuDanhmucService = dmDulieuDanhmucService;
            _quanLyMauTestService = quanLyMauTestService;
            _ILog = ILog;
            _IMapper = IMapper;
        }
        public ActionResult Index(long id)
        {
            var model = new IndexVM();
            model.Data = _TD_CauHinhSangLocUngVienService.GetDataByDotTuyenDungId(id);
            model.DotTuyenDungId = id;
            return View("Index", model);
        }

        public PartialViewResult Create(long id)
        {
            var model = new CreateVM();

            model.DotTuyenDungId = id;

            #region Gán thứ tự
            var listExistence = _TD_CauHinhSangLocUngVienService.GetDataByDotTuyenDungId(id);
            if (listExistence != null && listExistence.Count > 0)
            {
                model.Priority = listExistence.Count;
            }
            else
            {
                model.Priority = 1;
            } 
            #endregion

            ViewBag.DropdownType = ConstantExtension.GetDropdownData<CandidateSelectionTypeConstant>();
            ViewBag.DropdownTemplate = _quanLyMauTestService.GetDropdown("FileName", "Id");
            return PartialView("_CreatePartial", model);
        }
        [HttpPost]
        public JsonResult Create(CreateVM model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    var savedModel = _IMapper.Map<TD_CauHinhSangLocUngVien>(model);
                    _TD_CauHinhSangLocUngVienService.Create(savedModel);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _ILog.Error("Lỗi ở thêm mới cấu hình sàng lọc ứng viên", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id)
        {
            var existedModel = _TD_CauHinhSangLocUngVienService.GetById(id);
            if (existedModel == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin cấu hình");
            }
            var model = _IMapper.Map<TD_CauHinhSangLocUngVien, EditVM>(existedModel);
            ViewBag.DropdownType = ConstantExtension.GetDropdownData<CandidateSelectionTypeConstant>(model.Type);
            ViewBag.DropdownTemplate = _quanLyMauTestService.GetDropdown("FileName", "Id");
            ViewBag.TypeName = ConstantExtension.GetName<CandidateSelectionTypeConstant>(model.Type);
            return PartialView("_EditPartial", model);
        }
        [HttpPost]
        public JsonResult Edit(EditVM model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    var existedModel = _TD_CauHinhSangLocUngVienService.GetById(model.Id);
                    existedModel = _IMapper.Map(model, existedModel);
                    _TD_CauHinhSangLocUngVienService.Update(existedModel);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được!";
                _ILog.Error("Lỗi cập nhật chỉnh sửa cấu hình chung", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                TD_CauHinhSangLocUngVien entity = _TD_CauHinhSangLocUngVienService.GetById(id);
                if (entity != null)
                {
                    _TD_CauHinhSangLocUngVienService.Delete(entity);
                    result.Message = "Xóa cấu hình thành công";
                    _ILog.Info(result.Message);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Cấu hình không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa cấu hình không thành công";
                _ILog.Error("Xóa cấu hình không thành công", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult SavePriorityChanges(string jsonStr = "")
        {
            var result = new JsonResultBO(true);
            try
            {
                if (!String.IsNullOrEmpty(jsonStr))
                {
                    int priorityCounter = 1;
                    List<int> listObj = JsonConvert.DeserializeObject<List<int>>(jsonStr);

                    foreach (var obj in listObj)
                    {
                        var existed = _TD_CauHinhSangLocUngVienService.GetById(obj);
                        if (existed != null)
                        {
                            existed.Priority = priorityCounter++;
                            _TD_CauHinhSangLocUngVienService.Update(existed);
                        }
                    }
                    result.Message = "Cập nhật thay đổi thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Không có thay đổi";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không lưu được thay đổi";
                _ILog.Error("Lỗi lưu thay đổi thứ tự cấu hình sàng lọc ứng viên", ex);
            }
            return Json(result);
        }
        public JsonResult ReloadRow(long id)
        {
            var result = new JsonResultBO(true);

            try
            {
                result.Param = _TD_CauHinhSangLocUngVienService.GetDataByDotTuyenDungId(id);
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "Có lỗi xảy ra";
                _ILog.Error("Lỗi load lại dữ liệu cấu hình sàng lọc ứng viên", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}