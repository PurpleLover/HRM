using CommonHelper.String;
using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.ConfigRecruitmentRequestRepository;
using QLNS.Repository.DanhmucRepository;
using QLNS.Repository.RecruitmentRequestRepository;
using QLNS.Repository.RecruitmentSkillDetailRepository;
using QLNS.Repository.RecruitmentSkillRepository;
using QLNS.Service.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.ConfigRecruitmentRequestService
{
    public class ConfigRecruitmentRequestService : EntityService<ConfigRecruitmentRequest>, IConfigRecruitmentRequestService
    {
        IRecruitmentSkillRepository recruitmentSkillRepository;
        IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository;
        IConfigRecruitmentRequestRepository configRecruitmentRequestRepository;
        IDM_DulieuDanhmucRepository dataCategoryRepository;
        ILog logger;

        public ConfigRecruitmentRequestService(
            IUnitOfWork unitOfWork,
            IConfigRecruitmentRequestRepository configRecruitmentRequestRepository,
            IRecruitmentSkillRepository recruitmentSkillRepository,
            IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository,
            IDM_DulieuDanhmucRepository dataCategoryRepository,
            ILog logger)
            : base(unitOfWork, configRecruitmentRequestRepository)
        {
            this.configRecruitmentRequestRepository = configRecruitmentRequestRepository;
            this.recruitmentSkillRepository = recruitmentSkillRepository;
            this.recruitmentSkillDetailRepository = recruitmentSkillDetailRepository;
            this.dataCategoryRepository = dataCategoryRepository;
            this.logger = logger;
            this.logger.Info("Khởi tạo ConfigRecruitmentRequestService");
        }

        public IEnumerable<ConfigRecruitmentRequest> GetConfigByRequest(RecruitmentRequest request)
        {
            var groupSkillIds = request.SkillGroups.ToListNumber<long>(',');
            foreach (var groupSkillId in groupSkillIds)
            {
                //lấy các nhóm kỹ năng
                var entityGroupSkill = recruitmentSkillRepository.GetById(groupSkillId);
                if (entityGroupSkill != null && !string.IsNullOrEmpty(entityGroupSkill.Skills))
                {
                    //danh sách kỹ năng của từng nhóm
                    var skillIds = entityGroupSkill.Skills.ToListNumber<long>(',');
                    foreach (var skillId in skillIds)
                    {
                        var entitySkill = recruitmentSkillDetailRepository.GetById(skillId);
                        if (entitySkill != null)
                        {
                            ConfigRecruitmentRequest config = new ConfigRecruitmentRequest()
                            {
                                RequestId = request.Id,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                GroupSkillId = entityGroupSkill.Id,
                                SkillId = entitySkill.Id,
                            };
                            if (entitySkill.DataType == DataTypeConstant.CATEGORY && entitySkill.CategoryId != null)
                            {
                                var dataCategoryIds = dataCategoryRepository.FindBy(x => x.GroupId == entitySkill.CategoryId)
                                    .Select(x => x.Id).ToArray();
                                config.CategoryData = string.Join(",", dataCategoryIds);
                                config.CategoryId = entitySkill.CategoryId;
                            }
                            else if (entitySkill.DataType == DataTypeConstant.NUMBER)
                            {
                                config.AbsoluteNumber = entitySkill.AbsoluteNumber;
                            }
                            else if (entitySkill.DataType == DataTypeConstant.TEXT)
                            {
                                config.TextValue = entitySkill.TextValue;
                            }

                            if (!string.IsNullOrEmpty(config.CategoryData) || !string.IsNullOrEmpty(config.TextValue) || config.AbsoluteNumber != null)
                            {
                                yield return config;
                            }
                        }
                    }
                }
            }
        }
    }
}
