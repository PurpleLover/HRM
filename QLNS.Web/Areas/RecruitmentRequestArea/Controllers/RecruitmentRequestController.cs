using CommonHelper.ObjectExtention;
using CommonHelper.SQL;
using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ConfigRecruitmentRequestService;
using QLNS.Service.Constant;
using QLNS.Service.DepartmentService;
using QLNS.Service.DM_DulieuDanhmucService;
using QLNS.Service.DM_NhomDanhmucService;
using QLNS.Service.RecruitmentRequestService;
using QLNS.Service.RecruitmentRequestService.DTO;
using QLNS.Service.RecruitmentSkillDetailService;
using QLNS.Service.RecruitmentSkillService;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Service.Common.Constant;
using static QLNS.Web.Areas.RecruitmentRequestArea.Models.RecruitmentRequestViewModel;

namespace QLNS.Web.Areas.RecruitmentRequestArea.Controllers
{
    public class RecruitmentRequestController : BaseController
    {
        private readonly IConfigRecruitmentRequestService configRecruitmentRequestService;
        private readonly IRecruitmentRequestService recruitmentRequestService;
        private readonly IRecruitmentSkillService recruitmentSkillService;
        private readonly IRecruitmentSkillDetailService recruitmentSkillDetailService;
        private readonly IDepartmentService departmentService;
        private readonly IDM_NhomDanhmucService categoryGroupService;
        private readonly IDM_DulieuDanhmucService dataCategoryService;

        private readonly ILog logger;

        public RecruitmentRequestController(IRecruitmentRequestService recruitmentRequestService,
            IDepartmentService departmentService,
            IDM_NhomDanhmucService categoryGroupService,
            IRecruitmentSkillService recruitmentSkillService,
            IConfigRecruitmentRequestService configRecruitmentRequestService,
            IRecruitmentSkillDetailService recruitmentSkillDetailService,
            IDM_DulieuDanhmucService dataCategoryService,
            ILog logger)
        {
            this.recruitmentRequestService = recruitmentRequestService;
            this.departmentService = departmentService;
            this.categoryGroupService = categoryGroupService;
            this.recruitmentSkillService = recruitmentSkillService;
            this.configRecruitmentRequestService = configRecruitmentRequestService;
            this.recruitmentSkillDetailService = recruitmentSkillDetailService;
            this.dataCategoryService = dataCategoryService;
            this.logger = logger;
        }

        // GET: RecruitmentRequestArea/RecruitmentRequest
        public ActionResult Index()
        {
            var searchModel = new RecruitmentRequestSearchDTO();
            SessionManager.SetValue("RecruitmentRequestSearch", new RecruitmentRequestSearchDTO());
            RecruitmentRequestIndexViewModel viewModel = new RecruitmentRequestIndexViewModel()
            {
                GroupDepartments = departmentService.GetDropdown("Name", "Id"),
                GroupData = recruitmentRequestService.GetDataByPage(searchModel),
                GroupPositions = categoryGroupService.GetDataByCode(NHOM_DANHMUC_CONSTANT.VITRI)
            };
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("RecruitmentRequestSearch") as RecruitmentRequestSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RecruitmentRequestSearchDTO();
            }
            if (!string.IsNullOrEmpty(sortQuery))
            {
                searchModel.sortQuery = sortQuery;
            }
            if (pageSize > 0)
            {
                searchModel.pageSize = pageSize;
            }
            SessionManager.SetValue("RecruitmentRequestSearch", searchModel);
            var data = recruitmentRequestService.GetDataByPage(searchModel);
            return Json(data);
        }

        public PartialViewResult Edit(long id = 0)
        {
            var viewModel = new RecruitmentRequestEditViewModel();
            var editEntity = recruitmentRequestService.GetById(id) ?? new RecruitmentRequest();
            var groupSkillIds = editEntity.SkillGroups.ToListNumber<long>(',').Cast<object>().ToList();
            viewModel = new RecruitmentRequestEditViewModel()
            {
                Id = editEntity.Id,
                DepartmentId = editEntity.DepartmentId.ToString(),
                PositionId = editEntity.PositionId.ToString(),
                EstimateQuantity = editEntity.EstimateQuantity.ToString(),
                Title = editEntity.Title,
                Comment = editEntity.Comment,
                UntilDate = editEntity.Id > 0 ? editEntity.UntilDate : DateTime.Now,
                SkillIds = editEntity.SkillGroups.ToListNumber<long>(',').ToArray(),
                GroupDepartments = departmentService.GetDropdown("Name", "Id"),
                GroupPositions = categoryGroupService.GetDataByCode(NHOM_DANHMUC_CONSTANT.VITRI),
                GroupSkills = recruitmentSkillService.GetDropDownMultiple("Title", "Id", groupSkillIds),
                GroupTemplate = recruitmentRequestService.GetAll().Where(x => x.IsTemplate == true)
                .GetDropdown("Title", "Id", editEntity.TemplateId),
                TemplateId = editEntity.TemplateId,
                IsChooseFromTemplate = editEntity.TemplateId != null ? true : false
            };
            return PartialView("_EditPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("RecruitmentRequestSearch") as RecruitmentRequestSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RecruitmentRequestSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.QueryDepartmentId = form["QueryDepartmentId"].ToNullableNumber<int>();
            searchModel.QueryUntilDateFrom = form["QueryUntilDateFrom"];
            searchModel.QueryUntileDateTo = form["QueryUntileDateTo"];
            searchModel.QueryPositions = form["QueryPositions"];
            searchModel.QueryStatus = form["QueryStatus"];
            searchModel.QueryQuantityFrom = form["QueryQuantityFrom"].ToNullableNumber<int>();
            searchModel.QueryQuantityTo = form["QueryQuantityTo"].ToNullableNumber<int>();
            SessionManager.SetValue("RecruitmentRequestSearch", searchModel);
            var data = recruitmentRequestService.GetDataByPage(searchModel);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(RecruitmentRequestEditViewModel model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {
                    RecruitmentRequest entity = new RecruitmentRequest();
                    if (model.Id <= 0)
                    {
                        entity = new RecruitmentRequest()
                        {
                            Title = model.Title,
                            DepartmentId = model.DepartmentId.ToNumber<int>(),
                            PositionId = model.PositionId.ToNumber<int>(),
                            EstimateQuantity = model.EstimateQuantity.ToNumber<int>(),
                            Comment = model.Comment,
                            UntilDate = model.UntilDate.Value.ToStartDay(),
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            Status = YeuCauTuyenDungTrangThaiConst.MoiTao,
                            TemplateId = model.TemplateId != null ? model.TemplateId : null,
                            SkillGroups = model.SkillIds != null ? string.Join(",", model.SkillIds.ToArray()) : null
                        };
                        recruitmentRequestService.Create(entity);

                        //delete all old config
                        var oldConfig = configRecruitmentRequestService.FindBy(x => x.RequestId == entity.Id);
                        configRecruitmentRequestService.DeleteRange(oldConfig);

                        //add new config
                        if (!model.IsChooseFromTemplate)
                        {
                            var lattestConfig = configRecruitmentRequestService.GetConfigByRequest(entity);
                            SQLHelper.BulkInsert<ConfigRecruitmentRequest>(lattestConfig);
                        }
                        else
                        {
                            var configTemplateData = configRecruitmentRequestService.FindBy(x => x.RequestId == model.TemplateId);
                            var listConfig = new List<ConfigRecruitmentRequest>();
                            foreach(var configTemplate in configTemplateData)
                            {
                                configTemplate.Id = 0;
                                configTemplate.RequestId = entity.Id;
                                listConfig.Add(configTemplate);
                            }
                            SQLHelper.BulkInsert<ConfigRecruitmentRequest>(listConfig);
                        }
                        logger.InfoFormat("Thêm mới yêu cầu tuyển dụng {0}", model.Title);
                    }
                    else
                    {
                        entity = recruitmentRequestService.GetById(model.Id);
                        entity.Title = model.Title;
                        entity.DepartmentId = model.DepartmentId.ToNumber<int>();
                        entity.PositionId = model.PositionId.ToNumber<int>();
                        entity.EstimateQuantity = model.EstimateQuantity.ToNumber<int>();
                        entity.Comment = model.Comment;
                        entity.UntilDate = model.UntilDate.Value.ToStartDay();
                        entity.TemplateId = model.TemplateId != null ? model.TemplateId : null;
                        entity.SkillGroups = model.SkillIds != null ? string.Join(",", model.SkillIds.ToArray()) : null;
                        recruitmentRequestService.Update(entity);

                        logger.InfoFormat("Cập nhật yêu cầu tuyển dụng {0}", model.Title);
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
                logger.Error("Lỗi cập nhật thông tin Module", ex);
            }
            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                RecruitmentRequest entity = recruitmentRequestService.GetById(id);
                if (entity != null)
                {
                    recruitmentRequestService.Delete(entity);
                    result.Message = "Xóa yêu cầu tuyển dụng thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Yêu cầu tuyển dụng không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa yêu cầu tuyển dụng không thành công";
                logger.Error("Xóa yêu cầu tuyển dụng không thành công", ex);
            }
            return Json(result);
        }

        public ViewResult DetailRecruitmentRequest(long id)
        {
            RecruitmentRequestDetailViewModel viewModel = new RecruitmentRequestDetailViewModel()
            {
                EntityDTO = recruitmentRequestService.GetDataById(id),
                GroupSkills = recruitmentSkillService.GetConfigDataOfRequest(id),
                ConfigsData = configRecruitmentRequestService.FindBy(x => x.RequestId == id)
            };
            return View(viewModel);
        }

        public ViewResult ConfigRecruitmentRequest(long id)
        {
            RecruitmentRequestDetailViewModel viewModel = new RecruitmentRequestDetailViewModel()
            {
                Entity = recruitmentRequestService.GetById(id),
                GroupSkills = recruitmentSkillService.GetSkillGroupByRequestId(id),
                ConfigsData = configRecruitmentRequestService.FindBy(x => x.RequestId == id)
            };
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult SaveConfigRecruitmentRequest(FormCollection form)
        {
            JsonResultBO result = new JsonResultBO(true);
            try
            {
                //delete tất cả dữ liệu trong bảng config
                long requestId = form["REQUEST_ID"].ToNumber<long>();
                var oldConfig = configRecruitmentRequestService.FindBy(x => x.RequestId == requestId);
                configRecruitmentRequestService.DeleteRange(oldConfig);

                //các nhóm kỹ năng được config
                var groupSkillIds = form["GROUP_SKILL"].ToListNumber<long>(',');

                List<ConfigRecruitmentRequest> configs = new List<ConfigRecruitmentRequest>();

                foreach (var groupSkill in groupSkillIds)
                {
                    //các kỹ năng trong từng nhóm
                    var skills = form["INPUT_SKILL_" + groupSkill].ToListNumber<long>(',');

                    foreach (var skill in skills)
                    {
                        var entitySkill = recruitmentSkillDetailService.GetById(skill);
                        if (entitySkill != null)
                        {
                            ConfigRecruitmentRequest config = new ConfigRecruitmentRequest()
                            {
                                RequestId = requestId,
                                GroupSkillId = groupSkill,
                                SkillId = entitySkill.Id,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                            };

                            if (entitySkill.DataType == DataTypeConstant.CATEGORY)
                            {
                                config.CategoryId = entitySkill.CategoryId;
                                config.CategoryData = form["CHECKBOX_" + skill];
                            }
                            else if (entitySkill.DataType == DataTypeConstant.NUMBER)
                            {
                                config.AbsoluteNumber = form["NUMBER_" + skill].ToNullableNumber<int>();
                            }
                            else if (entitySkill.DataType == DataTypeConstant.TEXT)
                            {
                                config.TextValue = form["TEXT_" + skill].Trim();
                            }

                            if (config.AbsoluteNumber != null || !string.IsNullOrEmpty(config.TextValue) || !string.IsNullOrEmpty(config.CategoryData))
                            {
                                configs.Add(config);
                            }
                        }
                    }
                }

                if (configs.Any())
                {
                    SQLHelper.BulkInsert<ConfigRecruitmentRequest>(configs);
                }
                result.Message = "Cập nhật cấu hình yêu cầu tuyển dụng thành công";
            }
            catch (Exception ex)
            {
                logger.Error("Lỗi cấu hình dữ yêu cầu tuyển dụng", ex);
                result.Message = "Cấu hình dữ liệu không thành công";
                result.Status = false;
            }
            return Json(result);
        }

        public PartialViewResult GetSkillData(RecruitmentRequestEditViewModel model)
        {
            if (model.TemplateId == null)
            {
                return PartialView("_OptionalSkillData", model);
            }
            else
            {
                return PartialView("_TemplateSkillData", model);
            }
        }

        public PartialViewResult GetSkillDataOnChange(bool isUsingTemplate)
        {
            var model = new RecruitmentRequestEditViewModel()
            {
                GroupSkills = recruitmentSkillService.GetDropDownMultiple("Title", "Id"),
                GroupTemplate = recruitmentRequestService.GetAll().Where(x => x.IsTemplate == true)
                            .GetDropdown("Title", "Id")
            };

            if (!isUsingTemplate)
            {
                return PartialView("_OptionalSkillData", model);
            }
            else
            {
                return PartialView("_TemplateSkillData", model);
            }
        }

        

    }
}