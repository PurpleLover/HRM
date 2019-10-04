using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DM_DulieuDanhmucService;
using QLNS.Service.DM_DulieuDanhmucService.DTO;
using QLNS.Service.DM_NhomDanhmucService;
using QLNS.Web.Areas.DmDulieuDanhmucArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.DmDulieuDanhmucArea.Controllers
{
    public class DmDulieuDanhmucController : Controller
    {
        // GET: DmDulieuDanhmucArea/DmDulieuDanhmuc
        IDM_DulieuDanhmucService _dm_DulieuDanhmucService;
        IDM_NhomDanhmucService _dm_NhomDanhmucService;
        ILog _ILog;
        const string SessionSearchString = "DulieuDanhmucSearch";

        public DmDulieuDanhmucController(IDM_DulieuDanhmucService dm_DulieuDanhmucService, IDM_NhomDanhmucService dm_NhomdanhmucService, ILog ILog)
        {
            _dm_DulieuDanhmucService = dm_DulieuDanhmucService;
            _dm_NhomDanhmucService = dm_NhomdanhmucService;
            _ILog = ILog;
        }
        public ActionResult Index(long id)
        {
            var searchModel = new DM_DulieuDanhmucSearchDTO();
            SessionManager.SetValue(SessionSearchString, null);
            
            var groupName = _dm_NhomDanhmucService.GetById(id).GroupName;
            if (groupName==null)
            {
                return HttpNotFound();
            }
            var model = new IndexVM();
            model.GroupId = id;
            searchModel.GroupId = id;
            model.Data = _dm_DulieuDanhmucService.GetDataByPage(id, null);
            ViewBag.PageName = "Danh sách dữ liệu danh mục " + groupName;
            return View("Index", model);
        }

        [HttpPost]
        public JsonResult GetData(long groupid,int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as DM_DulieuDanhmucSearchDTO;
            if (searchModel == null)
            {
                searchModel = new DM_DulieuDanhmucSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _dm_DulieuDanhmucService.GetDataByPage(groupid, searchModel, indexPage, pageSize);
            return Json(data);
        }

        public PartialViewResult Create(long id)
        {
            var model = new CreateVM();
            model.GroupId = id;
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
                    if (!String.IsNullOrEmpty(model.Code) && _dm_DulieuDanhmucService.CheckCodeExisted(model.GroupId, model.Code.ToUpper()))
                    {
                        throw new Exception(String.Format("Mã nhóm {0} đã tồn tại!", model.Code.ToUpper()));
                    }
                    var dulieuDanhmuc = new DM_DulieuDanhmuc();
                    dulieuDanhmuc.GroupId = model.GroupId;
                    dulieuDanhmuc.Name = model.Name;
                    dulieuDanhmuc.Code = model.Code.ToUpper();
                    dulieuDanhmuc.Priority = model.Priority;
                    dulieuDanhmuc.Note = model.Note;
                    _dm_DulieuDanhmucService.Create(dulieuDanhmuc);
                    _ILog.Info(String.Format("Thêm mới nhóm danh mục {0}", dulieuDanhmuc.Name));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _ILog.Error("Lỗi ở nhóm danh mục", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id)
        {
            var model = new EditVM();
            var singleGroup = _dm_DulieuDanhmucService.GetById(id);
            if (singleGroup == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin");
            }
            model = new EditVM()
            {
                Id = singleGroup.Id,
                GroupId = singleGroup.GroupId,
                Name = singleGroup.Name,
                Code = singleGroup.Code,
                Priority = singleGroup.Priority,
                Note = singleGroup.Note
            };
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
                    var singleGroup = _dm_DulieuDanhmucService.GetById(model.Id);
                    if (singleGroup == null)
                    {
                        throw new Exception("Không tìm thấy nhóm danh mục");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(model.Code))
                        {
                            if (singleGroup.Code.Equals(model.Code.ToUpper()))
                            {
                                singleGroup.Name = model.Name;
                                singleGroup.Priority = model.Priority;
                                singleGroup.Note = model.Note;
                                _dm_DulieuDanhmucService.Update(singleGroup);
                            }
                            else if (_dm_DulieuDanhmucService.CheckCodeExisted(model.GroupId, model.Code.ToUpper()))
                            {
                                throw new Exception(String.Format("Mã nhóm {0} đã tồn tại", model.Code.ToUpper()));
                            }
                            else
                            {
                                singleGroup.Name = model.Name;
                                singleGroup.Code = model.Code.ToUpper();
                                singleGroup.Priority = model.Priority;
                                singleGroup.Note = model.Note;
                                _dm_DulieuDanhmucService.Update(singleGroup);
                            }
                        }
                        else
                        {
                            throw new Exception("Thiếu mã dữ liệu");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được!";
                _ILog.Error("Lỗi cập nhật chỉnh sửa nhóm danh mục", ex);
            }
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue(SessionSearchString) as DM_DulieuDanhmucSearchDTO;

            if (searchModel == null)
            {
                searchModel = new DM_DulieuDanhmucSearchDTO();
                searchModel.pageSize = 20;
            }

            searchModel.QueryName = form["QueryName"];
            searchModel.QueryCode = form["QueryCode"].ToUpper();
            SessionManager.SetValue(SessionSearchString, searchModel);
            var data = _dm_DulieuDanhmucService.GetDataByPage(searchModel.GroupId.GetValueOrDefault(),searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                DM_DulieuDanhmuc entity = _dm_DulieuDanhmucService.GetById(id);
                if (entity != null)
                {
                    _dm_DulieuDanhmucService.Delete(entity);
                    result.Message = "Xóa danh mục thành công";
                    _ILog.Info(result.Message);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Danh mục không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa danh mục không thành công";
                _ILog.Error("Xóa danh mục không thành công", ex);
            }
            return Json(result);
        }
    }
}