using AutoMapper;
using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ModuleService;
using QLNS.Service.OperationService;
using QLNS.Service.OperationService.DTO;
using QLNS.Web.Areas.OperationArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.OperationArea.Models.OperationViewModel;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;


/*
 * @author:duynn
 * @create_date: 19/04/2019
 */
namespace QLNS.Web.Areas.OperationArea.Controllers
{
    public class OperationController : BaseController
    {
        private readonly IOperationService _operationService;
        private readonly IModuleService _moduleService;
        private readonly ILog _Ilog;
        private readonly IMapper _mapper;

        public OperationController(IOperationService operationService, IModuleService mooduleService,
            IMapper mapper, ILog Ilog)
        {
            _mapper = mapper;
            _operationService = operationService;
            _moduleService = mooduleService;
            _Ilog = Ilog;
        }

        // GET: OperationArea/Operation
        public ActionResult Index(int moduleId)
        {
            var searchModel = new OperationSearchDTO()
            {
                QueryModuleId = moduleId
            };

            SessionManager.SetValue("OperationSearch", searchModel);
            OperationIndexViewModel viewModel = new OperationIndexViewModel()
            {
                ModuleId = moduleId,
                GroupData = _operationService.GetDataByPage(searchModel),
            };
            var name = _moduleService.GetById(moduleId);
            ViewBag.TenChucNang = name.Name;
            return View(viewModel);
        }


        [HttpPost]
        public JsonResult GetData(int indexPage, int moduleId, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("OperationSearch") as OperationSearchDTO;
            if (searchModel == null)
            {
                searchModel = new OperationSearchDTO();
            }
            searchModel.QueryModuleId = moduleId;
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("OperationSearch", searchModel);
            var data = _operationService.GetDataByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }

        public PartialViewResult Edit(int moduleId, long id = 0)
        {
            var viewModel = new OperationEditViewModel();
            var editEntity = _operationService.GetById(id) ?? new Operation() { ModuleId = moduleId, IsShow = true };
            viewModel = new OperationEditViewModel()
            {
                Id = editEntity.Id,
                Name = editEntity.Name,
                Code = editEntity.Code,
                URL = editEntity.URL,
                IsShow = editEntity.IsShow,
                Order = editEntity.Order.ToString(),
                ModuleId = moduleId
            };
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("OperationSearch") as OperationSearchDTO;

            if (searchModel == null)
            {
                searchModel = new OperationSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.QueryModuleId = int.Parse(form["QUERY_MODULE_ID"]);
            searchModel.QueryName = form["QUERY_NAME"];
            searchModel.QueryIsShow = !string.IsNullOrEmpty(form["QUERY_SHOW"]) ? (bool?)(int.Parse(form["QUERY_SHOW"]) > 0) : null;
            SessionManager.SetValue("OperationSearch", searchModel);
            var data = _operationService.GetDataByPage(searchModel, 1);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(OperationEditViewModel model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        Operation entity = new Operation()
                        {
                            Name = model.Name,
                            Code = model.Code,
                            URL = model.URL,
                            IsShow = model.IsShow,
                            Order = model.Order.ToNumber<int>(),
                            ModuleId = model.ModuleId
                        };
                        _operationService.Create(entity);
                        _Ilog.InfoFormat("Thêm mới thao tác {0}", model.Name);
                    }
                    else
                    {
                        Operation entity = _operationService.GetById(model.Id);
                        entity.Name = model.Name;
                        entity.Code = model.Code;
                        entity.URL = model.URL;
                        entity.IsShow = model.IsShow;
                        entity.Order = model.Order.ToNumber<int>();
                        _operationService.Update(entity);

                        _Ilog.InfoFormat("Cập nhật thao tác {0}", model.Name);
                    }
                    return Json(result);
                }
                result.Message = ModelState.GetErrors();
                result.Status = false;
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được";
                _Ilog.Error("Lỗi cập nhật thông tin Thao tác", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                Operation entity = _operationService.GetById(id);
                if (entity != null)
                {
                    _operationService.Delete(entity);
                    result.Message = "Xóa thao tác thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Thao tác không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa thao tác không thành công";
                _Ilog.Error("Xóa thao tác không thành công", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddOperation(string url)
        {
            var listOperation = _operationService.GetAll().ToList();
            var listModule = _moduleService.GetAll().ToList();


            var model = new AddMenuViewModel();
            model.EditViewModel = _mapper.Map<OperationViewModel.OperationEditViewModel>(listOperation.Where(x => x.URL != null && x.URL.ToLower().Equals(url.ToLower())).FirstOrDefault());
            if (model.EditViewModel == null)
            {
                model.EditViewModel = new OperationEditViewModel()
                {
                    URL = url
                };
            }
            model.ListModule = listModule.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.ListModule.Add(new SelectListItem() { Text = "--Chọn Module--",Value="" });
            model.ListPermissionCode = ConstantExtension.GetDropdownData<PermissionCodeConst>(model.EditViewModel?.Code).Where(x => listOperation.Any(a => a.Code != x.Value)).ToList();
            model.ListPermissionCode.Add(new SelectListItem() { Text = "--Chọn Mã--", Value = "" });
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveAddMenu(AddMenuViewModel modelAll)
        {
            var result = new JsonResultBO(true, "Thiết lập thành công");
            var model = modelAll.EditViewModel;
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results);
            if (isValid)
            {
                if (_operationService.CheckCode(model.Code, model.Id))
                {
                    result.MessageFail("Mã thao tác đã tồn tại");
                    return Json(result);
                }
                if (model.Id>0)
                {
                    var objDB = _operationService.GetById(model.Id);
                    if (objDB==null)
                    {
                        result.MessageFail("Không tìm thấy thao tác");
                        return Json(result);
                    }
                    _operationService.Update(_mapper.Map<OperationEditViewModel, Operation>(model, objDB));

                }
                else
                {
                    _operationService.Create(_mapper.Map<Operation>(model));
                }
            }
            return Json(result);

        }
    }
}