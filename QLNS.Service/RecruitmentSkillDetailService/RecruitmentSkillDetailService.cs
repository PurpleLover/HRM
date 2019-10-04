using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.RecruitmentSkillDetailRepository;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using QLNS.Service.RecruitmentSkillDetailService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using QLNS.Repository.DanhmucRepository;

namespace QLNS.Service.RecruitmentSkillDetailService
{
    public class RecruitmentSkillDetailService : EntityService<RecruitmentSkillDetail>, IRecruitmentSkillDetailService
    {
        IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository;
        IDM_DulieuDanhmucRepository categoryDataRepository;
        IDM_NhomDanhmucRepository categoryRepository;
        ILog logger;

        public RecruitmentSkillDetailService(IUnitOfWork unitOfWork, 
            IRecruitmentSkillDetailRepository recruitmentSkillDetailRepository,
            IDM_DulieuDanhmucRepository categoryDataRepository,
            IDM_NhomDanhmucRepository categoryRepository,
        ILog logger) :
            base(unitOfWork, recruitmentSkillDetailRepository)
        {
            this.recruitmentSkillDetailRepository = recruitmentSkillDetailRepository;
            this.categoryDataRepository = categoryDataRepository;
            this.logger = logger;
            this.categoryRepository = categoryRepository;
            this.logger.Info("Khởi tạo RecruitmentSkillDetailService");
        }

        public PageListResultBO<RecruitmentSkillDetailDTO> GetDataByPage(RecruitmentSkillDetailSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var queryResult = (from skill in recruitmentSkillDetailRepository.GetAllAsQueryable()
                               join categoryData in categoryRepository.GetAllAsQueryable()
                               on skill.CategoryId equals categoryData.Id
                               into groupSkilCategory
                               from g1 in groupSkilCategory.DefaultIfEmpty()
                               select new RecruitmentSkillDetailDTO()
                               {
                                   Id = skill.Id,
                                   Name = skill.Name,
                                   DataType = skill.DataType,
                                   AbsoluteNumber = skill.AbsoluteNumber,
                                   CategoryId = skill.CategoryId,
                                   DataTypeValue = skill.DataType == DataTypeConstant.CATEGORY ? g1.GroupName : (skill.DataType == DataTypeConstant.NUMBER ? skill.AbsoluteNumber.ToString() : string.Empty) 
                               });
            if(searchParams != null)
            {
                if (!string.IsNullOrEmpty(searchParams.QueryName))
                {
                    searchParams.QueryName = searchParams.QueryName.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Name.Trim().ToLower().Contains(searchParams.QueryName));
                }

                if (searchParams.QueryType != null)
                {
                    queryResult = queryResult.Where(x => x.DataType == searchParams.QueryType);
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

            var result = new PageListResultBO<RecruitmentSkillDetailDTO>();
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
                var categoryData = categoryDataRepository.GetAll();
                result.ListItem.ForEach(x =>
                {
                    x.DataTypeName = ConstantExtension.GetName<DataTypeConstant>(x.DataType.ToString());
                    if(x.DataType == DataTypeConstant.CATEGORY && x.CategoryId != null)
                    {
                        x.GroupCategoryData = categoryData.Where(y => y.GroupId == x.CategoryId);
                    }
                });
            }
            return result;
        }
    }
}
