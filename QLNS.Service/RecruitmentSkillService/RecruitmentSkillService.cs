using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.RecruitmentSkillRepository;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentSkillService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using PagedList;
using QLNS.Repository.RecruitmentRequestRespository;
using CommonHelper.String;
using AutoMapper;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using QLNS.Repository.RecruitmentSkillDetailRepository;
using QLNS.Service.Constant;
using QLNS.Repository.DanhmucRepository;
using QLNS.Repository.ConfigRecruitmentRequestRepository;

namespace QLNS.Service.RecruitmentSkillService
{
    public class RecruitmentSkillService : EntityService<RecruitmentSkill>, IRecruitmentSkillService
    {
        IRecruitmentSkillRepository recruitmentSkillRepository;
        IRecruitmentRequestRepository recruitmentRequestRepository;
        IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository;
        IDM_DulieuDanhmucRepository dataCategoryRepository;
        IConfigRecruitmentRequestRepository configRecruitmentRequestRepository;

        ILog logger;
        IMapper mapper;
        public RecruitmentSkillService(IUnitOfWork unitOfWork,
            IRecruitmentSkillRepository recruitmentSkillRepository,
            IRecruitmentRequestRepository recruitmentRequestRepository,
            IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository,
            IDM_DulieuDanhmucRepository dataCategoryRepository,
            IConfigRecruitmentRequestRepository configRecruitmentRequestRepository,
            IMapper mapper,
            ILog logger) :
            base(unitOfWork, recruitmentSkillRepository)
        {
            this.recruitmentRequestRepository = recruitmentRequestRepository;
            this.recruitmentSkillRepository = recruitmentSkillRepository;
            this.recruitmentSkillDetailRepository = recruitmentSkillDetailRepository;
            this.dataCategoryRepository = dataCategoryRepository;
            this.configRecruitmentRequestRepository = configRecruitmentRequestRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// @author:duynn
        /// @description: lấy dữ liệu cấu hình yêu cầu tuyển dụng
        /// </summary>
        /// <param name="requestId">mã yêu cầu tuyển dụng</param>
        /// <returns></returns>
        public List<RecruitmentSkillDTO> GetConfigDataOfRequest(long requestId)
        {
            var result = new List<RecruitmentSkillDTO>();
            var configData = configRecruitmentRequestRepository.FindBy(x => x.RequestId == requestId);
            if (configData.Any())
            {
                //danh sách các nhóm kỹ năng
                var groupSkillIds = configData.Select(x => x.GroupSkillId).Distinct();

                foreach (var groupSkill in groupSkillIds)
                {
                    var groupSkillDTO = mapper.Map<RecruitmentSkill, RecruitmentSkillDTO>(recruitmentSkillRepository.GetById(groupSkill));
                    groupSkillDTO.GroupSkillDetails = new List<RecruitmentSkillDetailDTO>();

                    //danh sách kỹ năng
                    var skillIds = configData.Where(x => x.GroupSkillId == groupSkill)
                        .Select(x => x.SkillId);

                    foreach (var skill in skillIds)
                    {
                        var configSkill = configData.Where(x => x.RequestId == requestId && x.GroupSkillId == groupSkill && x.SkillId == skill)
                            .FirstOrDefault();
                        if (configSkill != null)
                        {
                            //lấy kỹ năng trong db
                            var skillDTO = mapper.Map<RecruitmentSkillDetail, RecruitmentSkillDetailDTO>(recruitmentSkillDetailRepository.GetById(skill));
                            if (skillDTO != null)
                            {
                                if (configSkill.CategoryData != null)
                                {
                                    var categoryDataIds = configSkill.CategoryData.ToListNumber<long>(',');
                                    skillDTO.GroupCategoryData = dataCategoryRepository.FindBy(x => categoryDataIds.Contains(x.Id));
                                }
                                else if (configSkill.AbsoluteNumber != null)
                                {
                                    skillDTO.AbsoluteNumber = configSkill.AbsoluteNumber;
                                }
                                else if (!string.IsNullOrEmpty(configSkill.TextValue))
                                {
                                    skillDTO.TextValue = configSkill.TextValue;
                                }
                                groupSkillDTO.GroupSkillDetails.Add(skillDTO);
                            }
                        }
                    }
                    //add nhóm kỹ năng vào collection
                    result.Add(groupSkillDTO);
                }
            }
            return result;
        }

        public RecruitmentSkillDTO GetDataById(long id)
        {
            var result = (from skill in recruitmentSkillRepository.GetAll()
                          where skill.Id == id
                          select mapper.Map<RecruitmentSkill, RecruitmentSkillDTO>(skill)
                          ).FirstOrDefault();
            return result;
        }

        public PageListResultBO<RecruitmentSkillDTO> GetDataByPage(RecruitmentSkillSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var queryResult = (from skill in recruitmentSkillRepository.GetAllAsQueryable()
                               select new RecruitmentSkillDTO()
                               {
                                   Id = skill.Id,
                                   Title = skill.Title,
                                   Skills = skill.Skills
                               });
            if (searchParams != null)
            {
                if (!string.IsNullOrEmpty(searchParams.QueryTitle))
                {
                    searchParams.QueryTitle = searchParams.QueryTitle.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Title.Trim().ToLower().Contains(searchParams.QueryTitle));
                }

                if (!string.IsNullOrEmpty(searchParams.sortQuery))
                {
                    queryResult = queryResult.OrderBy(searchParams.sortQuery);
                }
                else
                {
                    queryResult = queryResult.OrderByDescending(x => x.Id);
                }
            }
            var result = new PageListResultBO<RecruitmentSkillDTO>();
            if (pageSize == -1)
            {
                var pagedList = queryResult.ToList();
                result.Count = pagedList.Count;
                result.TotalPage = 1;
                result.ListItem = pagedList;
            }
            else
            {
                var dataPageList = queryResult.ToPagedList(pageIndex, pageSize);
                result.Count = dataPageList.TotalItemCount;
                result.TotalPage = dataPageList.PageCount;
                result.ListItem = dataPageList.ToList();
            }

            if (result.ListItem.Any())
            {
                var groupSkillsDetail = recruitmentSkillDetailRepository.GetAll();
                result.ListItem.ForEach(x =>
                {
                    x.NumberOfSkills = x.Skills.Split(',').Count();
                    var skillIds = x.Skills.ToListNumber<long>();
                    x.GroupSkillDetails = groupSkillsDetail.Where(y => skillIds.Contains(y.Id))
                        .Select(y => mapper.Map<RecruitmentSkillDetail, RecruitmentSkillDetailDTO>(y)).ToList();
                });
            }
            return result;
        }

        public List<RecruitmentSkillDTO> GetSkillGroupByRequestId(long requestId)
        {
            List<RecruitmentSkillDTO> result = new List<RecruitmentSkillDTO>();

            var request = recruitmentRequestRepository.GetById(requestId);
            if (request != null)
            {
                string skillGroups = null;
                if(request.TemplateId != null)
                {
                    var template = recruitmentRequestRepository.GetById(request.TemplateId);
                    if(template == null)
                    {
                        return result;
                    }
                    skillGroups = template.SkillGroups;
                }
                else
                {
                    if(request.SkillGroups == null)
                    {
                        return result;
                    }
                    skillGroups = request.SkillGroups;
                }

                IEnumerable<long> groupSkillIds = skillGroups.ToListNumber<long>(',');
                foreach (var item in groupSkillIds)
                {
                    //nhóm kỹ năng
                    var groupSkill = this.GetDataById(item);
                    if (groupSkill != null)
                    {
                        groupSkill.GroupSkillDetails = new List<RecruitmentSkillDetailDTO>();
                        if (!string.IsNullOrEmpty(groupSkill.Skills))
                        {
                            //các kỹ năng thuộc nhóm kỹ năng
                            var skillDetailIds = groupSkill.Skills.ToListNumber<long>(',');
                            foreach (var skillDetailId in skillDetailIds)
                            {
                                var dbSkillDetail = recruitmentSkillDetailRepository.GetById(skillDetailId);
                                if (dbSkillDetail != null)
                                {
                                    var dtoSkillDetail = mapper.Map<RecruitmentSkillDetail, RecruitmentSkillDetailDTO>(dbSkillDetail);
                                    //lấy danh sách danh mục có trong database
                                    if (dtoSkillDetail.DataType == DataTypeConstant.CATEGORY)
                                    {
                                        dtoSkillDetail.GroupCategoryData = dataCategoryRepository.FindBy(x => x.GroupId == dtoSkillDetail.CategoryId).ToList();
                                    }
                                    groupSkill.GroupSkillDetails.Add(dtoSkillDetail);
                                }
                            }
                        }
                        result.Add(groupSkill);
                    }
                }
            }
            return result;
        }
    }
}
