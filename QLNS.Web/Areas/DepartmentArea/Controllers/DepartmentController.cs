using log4net;
using Newtonsoft.Json;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.DepartmentService;
using QLNS.Service.DepartmentService.DTO;
using QLNS.Web.Areas.DepartmentArea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Areas.DepartmentArea.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: DepartmentArea/Department
        IDepartmentService _departmentService;
        ILog _ILog;
        const string SessionSearchString = "DepartmentSearch";

        public DepartmentController(IDepartmentService departmentService, ILog ILog)
        {
            _departmentService = departmentService;
            _ILog = ILog;
        }
        public ActionResult Index()
        {
            var searchModel = new DepartmentDTO();
            SessionManager.SetValue(SessionSearchString, null);
            var data = _departmentService.GetTree();
            return View("Index", data);
        }
        public JsonResult ReloadTree()
        {
            var result = new JsonResultBO(true);
            try
            {
                result.Param = _departmentService.GetTree();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không lấy được dữ liệu!";
                _ILog.Error("Lỗi tải lại cây phòng ban", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Lưu thay đổi thứ tự
        [HttpPost]
        public JsonResult SaveChanges(string jsonStr = "")
        {
            var result = new JsonResultBO(true);
            try
            {
                if (!String.IsNullOrEmpty(jsonStr))
                {
                    // Đánh thứ tự
                    long priorityCounter = 1;
                    // Cây từ Client
                    List<DepartmentCM> listObj = JsonConvert.DeserializeObject<List<DepartmentCM>>(jsonStr);
                    // Nút gốc
                    Department rootObj = _departmentService.FindBy(x => x.ParentId == null).FirstOrDefault();

                    foreach (DepartmentCM obj in listObj)
                    {
                        // Object thuộc tầng đầu tiên
                        Department floorModel = _departmentService.GetById(obj.id); 
                        
                        // Nếu object đứng cùng hàng với object gốc -> thành con object gốc
                        if (floorModel.ParentId != null)
                        {
                            floorModel.ParentId = rootObj.Id;
                            floorModel.Level = rootObj.Level + 1;
                        }
                        floorModel.Priority = priorityCounter++;
                        _departmentService.Update(floorModel);
                        if (obj.children != null && obj.children.Count > 0)
                        {
                            UpdateChild(ref priorityCounter, obj);
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
                _ILog.Error("Lỗi lưu thay đổi chỉnh sửa cây phòng ban", ex);
            }
            return Json(result);
        }
        private void UpdateChild(ref long priorityCounter, DepartmentCM obj)
        {
            foreach (DepartmentCM child in obj.children)
            {
                Department nextModel = _departmentService.GetById(child.id);
                nextModel.ParentId = obj.id;
                nextModel.Level = _departmentService.FindBy(x => x.Id == obj.id).Select(x => x.Level).FirstOrDefault() + 1;
                nextModel.Priority = priorityCounter++;
                _departmentService.Update(nextModel);
                if (child.children != null)
                {
                    UpdateChild(ref priorityCounter, child);
                }
            }
        }
        #endregion

        public PartialViewResult Create(long id = 0)
        {
            var model = new CreateVM();
            if (id > 0)
            {
                model.ParentId = id;
            }
            ViewBag.ListDepartment = _departmentService.GetDropdown("Name", "Id", (long)id);
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
                    if (!String.IsNullOrEmpty(model.Code) && _departmentService.CheckCodeExisted(model.Code.ToUpper()))
                    {
                        throw new Exception(String.Format("Mã phòng ban {0} đã tồn tại!", model.Code.ToUpper()));
                    }
                    var savedModel = new Department();
                    savedModel.Name = model.Name;
                    savedModel.Code = model.Code.ToUpper();
                    savedModel.ParentId = model.ParentId;
                    savedModel.Level = model.ParentId.HasValue
                        ? _departmentService.FindBy(x => x.Id == model.ParentId).Select(x => x.Level).FirstOrDefault() + 1
                        : 1;
                    _departmentService.Create(savedModel);
                    _ILog.Info(String.Format("Thêm mới phòng ban {0}", savedModel.Name));
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                _ILog.Error("Lỗi khởi tạo phòng ban", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id = 0)
        {
            var model = new EditVM();
            var existedModel = _departmentService.GetById(id);
            if (existedModel == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin phòng ban");
            }
            model = new EditVM()
            {
                Id = existedModel.Id,
                Name = existedModel.Name,
                Code = existedModel.Code,
                ParentId = existedModel.ParentId,
                Level = existedModel.Level
            };
            ViewBag.ListDepartment = _departmentService.GetDropdown("Name", "Id", model.ParentId);
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
                    var existedModel = _departmentService.GetById(model.Id);
                    if (existedModel == null)
                    {
                        throw new Exception("Không tìm thấy phòng ban");
                    }
                    else
                    {
                        if (existedModel.Code.Equals(model.Code.ToUpper()))
                        {
                            existedModel.ParentId = model.ParentId;
                            existedModel.Name = model.Name;
                            existedModel.Level = _departmentService.FindBy(x => x.Id == model.ParentId).Select(x => x.Level).FirstOrDefault() + 1;
                        }
                        else if (_departmentService.CheckCodeExisted(model.Code.ToUpper()))
                        {
                            throw new Exception(String.Format("Mã phòng ban {0} đã tồn tại", model.Code.ToUpper()));
                        }
                        else
                        {
                            existedModel.Name = model.Name;
                            existedModel.Code = model.Code.ToUpper();
                            existedModel.ParentId = model.ParentId;
                            existedModel.Level = _departmentService.FindBy(x => x.Id == model.ParentId).Select(x => x.Level).FirstOrDefault() + 1;
                        }
                        _departmentService.Update(existedModel);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được!";
                _ILog.Error("Lỗi chỉnh sửa phòng ban", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                Department entity = _departmentService.GetById(id);
                if (entity != null)
                {
                    // Chỉ xoá khi nút được trỏ tới là nút lá (không có con)
                    List<Department> entityChildren = _departmentService.FindBy(x => x.ParentId == entity.Id).ToList();
                    if (entityChildren != null && entityChildren.Count > 0)
                    {
                        result.Status = false;
                        result.Message = "Không thể xoá phòng ban";
                        _ILog.Info(result.Message);
                    }
                    else
                    {
                        _departmentService.Delete(entity);
                        result.Message = "Xóa phòng ban thành công";
                        _ILog.Info(result.Message);
                    }
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
                result.Message = "Xóa phòng ban không thành công";
                _ILog.Error("Xóa phòng ban không thành công", ex);
            }
            return Json(result);
        }
    }
}