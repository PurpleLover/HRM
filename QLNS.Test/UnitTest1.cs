using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CommonHelper.String;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QLNS.Model;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.DanhmucRepository;
using QLNS.Repository.RecruitmentRequestRespository;
using QLNS.Repository.RecruitmentSkillDetailRepository;
using QLNS.Repository.RecruitmentSkillRepository;
using QLNS.Service.Constant;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using QLNS.Service.RecruitmentSkillService;
using QLNS.Service.RecruitmentSkillService.DTO;

namespace QLNS.Test
{
    [TestClass]
    public class UnitTest1
    {
        DbContext context;
        IRecruitmentRequestRepository recruitmentRequestRepository;
        IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository;
        IRecruitmentSkillRepository recruitmentSkillRepository;
        IDM_DulieuDanhmucRepository dataCategoryRepository;
        IMapper mapper;

        [TestMethod]
        public void Can_Get_Info_RecruitmentRequest()
        {
            //arrange
            int id = 2;
            context = new QLNSContext();
            recruitmentRequestRepository = new RecruitmentRequestRepository(context);
            recruitmentSkillDetailRepository = new RecruitmentSkillDetailRepository(context);
            recruitmentSkillRepository = new RecruitmentSkillRepository(context);
            dataCategoryRepository = new DM_DulieuDanhmucRepository(context);
            //act
            List<RecruitmentSkillDTO> result = new List<RecruitmentSkillDTO>();

            var request = recruitmentRequestRepository.GetById(id);
            if (request != null && !string.IsNullOrEmpty(request.SkillGroups))
            {
                IEnumerable<long> groupSkillIds = request.SkillGroups.ToListNumber<long>(',');
                foreach (var item in groupSkillIds)
                {
                    //nhóm kỹ năng
                    var groupSkill = recruitmentSkillRepository.GetById(item);

                    var groupSkillDTO = new RecruitmentSkillDTO()
                    {
                        Id = groupSkill.Id,
                        Title = groupSkill.Title,
                        Skills = groupSkill.Skills
                    };

                    if (groupSkill != null)
                    {
                        groupSkillDTO.GroupSkillDetails = new List<RecruitmentSkillDetailDTO>();
                        if (!string.IsNullOrEmpty(groupSkill.Skills))
                        {
                            //các kỹ năng thuộc nhóm kỹ năng
                            var skillDetailIds = groupSkill.Skills.ToListNumber<long>(',');
                            foreach (var skillDetailId in skillDetailIds)
                            {
                                var dbSkillDetail = recruitmentSkillDetailRepository.GetById(skillDetailId);
                                if (dbSkillDetail != null)
                                {
                                    var dtoSkillDetail = new RecruitmentSkillDetailDTO()
                                    {
                                        Id = dbSkillDetail.Id,
                                        Name = dbSkillDetail.Name,
                                        DataType = dbSkillDetail.DataType,
                                        CategoryId = dbSkillDetail.CategoryId,
                                        AbsoluteNumber = dbSkillDetail.AbsoluteNumber,
                                    };
                                    //lấy danh sách danh mục có trong database
                                    if (dtoSkillDetail.DataType == DataTypeConstant.CATEGORY)
                                    {
                                        dtoSkillDetail.GroupCategoryData = dataCategoryRepository.FindBy(x => x.GroupId == dtoSkillDetail.CategoryId).ToList();
                                    }
                                    groupSkillDTO.GroupSkillDetails.Add(dtoSkillDetail);
                                }
                            }
                        }
                        result.Add(groupSkillDTO);
                    }
                }
            }

            //assert
            Assert.IsNotNull(result);
        }
    }
}
