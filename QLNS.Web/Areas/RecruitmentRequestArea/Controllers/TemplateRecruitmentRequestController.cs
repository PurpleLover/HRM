using CommonHelper.SQL;
using CommonHelper.String;
using log4net;
using log4net.Core;
using QLNS.Model.Entities;
using QLNS.Service.Common;
using QLNS.Service.ConfigRecruitmentRequestService;
using QLNS.Service.RecruitmentRequestService;
using QLNS.Service.RecruitmentRequestService.DTO;
using QLNS.Service.RecruitmentSkillService;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QLNS.Web.Areas.RecruitmentRequestArea.Models.RecruitmentRequestViewModel;
using static QLNS.Web.Areas.RecruitmentRequestArea.Models.TemplateRecruitmentRequestViewModel;

namespace QLNS.Web.Areas.RecruitmentRequestArea.Controllers
{
    public class TemplateRecruitmentRequestController : BaseController
    {
        IRecruitmentRequestService recruitmentRequestService;
        IRecruitmentSkillService recruitmentSkillService;
        IConfigRecruitmentRequestService configRecruitmentRequestService;
        ILog logger;

        public TemplateRecruitmentRequestController(
            IRecruitmentRequestService recruitmentRequestService,
            IRecruitmentSkillService recruitmentSkillService,
            IConfigRecruitmentRequestService configRecruitmentRequestService,
            ILog logger)
        {
            this.recruitmentRequestService = recruitmentRequestService;
            this.recruitmentSkillService = recruitmentSkillService;
            this.configRecruitmentRequestService = configRecruitmentRequestService;
            this.logger = logger;
        }

        // GET: RecruitmentRequestArea/TemplateRecruitmentRequest
        public ActionResult Index()
        {
            var searchModel = new RecruitmentRequestSearchDTO()
            {
                QueryTemplate = true
            };
            SessionManager.SetValue("TemplateRecruitmentRequestSearch", searchModel);
            TemplateRecruitmentRequestIndexViewModel viewModel = new TemplateRecruitmentRequestIndexViewModel()
            {
                GroupData = recruitmentRequestService.GetDataByPage(searchModel),
            };
            return View(viewModel);
        }


        [HttpPost]
        public JsonResult GetData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("TemplateRecruitmentRequestSearch") as RecruitmentRequestSearchDTO;
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
            searchModel.QueryTemplate = true;
            SessionManager.SetValue("TemplateRecruitmentRequestSearch", searchModel);
            var data = recruitmentRequestService.GetDataByPage(searchModel);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchData(FormCollection form)
        {
            var searchModel = SessionManager.GetValue("TemplateRecruitmentRequestSearch") as RecruitmentRequestSearchDTO;
            if (searchModel == null)
            {
                searchModel = new RecruitmentRequestSearchDTO();
                searchModel.pageSize = 20;
            }
            searchModel.QueryTitle = form["QueryTitle"];
            searchModel.QueryTemplate = true;
            SessionManager.SetValue("TemplateRecruitmentRequestSearch", searchModel);
            var data = recruitmentRequestService.GetDataByPage(searchModel);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(TemplateRecruitmentRequestEditViewModel model)
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
                            DepartmentId = 0,
                            PositionId = 0,
                            EstimateQuantity = 0,
                            Comment = String.Empty,
                            UntilDate = DateTime.Now,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            Status = DateTime.Now.ToShortDateString(),
                            IsTemplate = true,
                            SkillGroups = string.Join(",", model.SkillIds.ToArray())
                        };
                        recruitmentRequestService.Create(entity);

                        //delete all old config
                        var oldConfig = configRecruitmentRequestService.FindBy(x => x.RequestId == entity.Id);
                        configRecruitmentRequestService.DeleteRange(oldConfig);

                        //add new config
                        var lattestConfig = configRecruitmentRequestService.GetConfigByRequest(entity);
                        SQLHelper.BulkInsert<ConfigRecruitmentRequest>(lattestConfig);
                        logger.InfoFormat("Thêm mới biểu mẫu yêu cầu tuyển dụng {0}", model.Title);
                    }
                    else
                    {
                        entity = recruitmentRequestService.GetById(model.Id);
                        entity.Title = model.Title;
                        entity.SkillGroups = string.Join(",", model.SkillIds.ToArray());
                        recruitmentRequestService.Update(entity);

                        logger.InfoFormat("Cập nhật biểu mẫu yêu cầu tuyển dụng {0}", model.Title);
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
                logger.Error("Lỗi cập nhật thông tin biểu mẫu yêu cầu tuyển dụng", ex);
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
                    result.Message = "Xóa biểu mẫu yêu cầu tuyển dụng thành công";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Biểu mẫu yêu cầu tuyển dụng không tồn tại";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Xóa biểu mẫu yêu cầu tuyển dụng không thành công";
                logger.Error("Xóa biểu mẫu yêu cầu tuyển dụng không thành công", ex);
            }
            return Json(result);
        }

        public PartialViewResult Edit(long id = 0)
        {
            var viewModel = new TemplateRecruitmentRequestEditViewModel();
            var editEntity = recruitmentRequestService.GetById(id) ?? new RecruitmentRequest();
            var groupSkillIds = editEntity.SkillGroups.ToListNumber<long>(',').Cast<object>().ToList();
            viewModel = new TemplateRecruitmentRequestEditViewModel()
            {
                Id = editEntity.Id,
                Title = editEntity.Title,
                SkillIds = editEntity.SkillGroups.ToListNumber<long>(',').ToArray(),
                GroupSkills = recruitmentSkillService.GetDropDownMultiple("Title", "Id", groupSkillIds)
            };
            return PartialView("_EditPartial", viewModel);
        }
    }
}