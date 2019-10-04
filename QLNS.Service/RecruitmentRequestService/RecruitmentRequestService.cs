using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.DanhmucRepository;
using QLNS.Repository.RecruitmentRequestRespository;
using QLNS.Service.Common;
using QLNS.Service.RecruitmentRequestService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using CommonHelper.String;
using QLNS.Service.DepartmentService;
using QLNS.Repository.DepartmentRepository;
using QLNS.Repository.RecruitmentSkillRepository;
using QLNS.Service.Constant;
using AutoMapper;

namespace QLNS.Service.RecruitmentRequestService
{
    public class RecruitmentRequestService : EntityService<RecruitmentRequest>, IRecruitmentRequestService
    {
        ILog logger;
        IRecruitmentRequestRepository recruitmentRequestRepository;
        IDM_DulieuDanhmucRepository categoryDataRepository;
        IDepartmentRepository departmentRepository;
        IRecruitmentSkillRepository recruitmentSkillRepository;
        IMapper _mapper;
        public RecruitmentRequestService(IUnitOfWork unitOfWork,
            IRecruitmentRequestRepository recruitmentRequestRepository,
            IDM_DulieuDanhmucRepository categoryDataRepository,
            IDepartmentRepository departmentRepository,
            IRecruitmentSkillRepository recruitmentSkillRepository,
            IMapper mapper,
        ILog logger
            ) : base(unitOfWork, recruitmentRequestRepository)
        {
            this.recruitmentRequestRepository = recruitmentRequestRepository;
            this.departmentRepository = departmentRepository;
            this.categoryDataRepository = categoryDataRepository;
            this.recruitmentSkillRepository = recruitmentSkillRepository;
            this.logger = logger;
            _mapper = mapper;
        }

        public RecruitmentRequestDTO GetDataById(long id)
        {
            var result = (from request in recruitmentRequestRepository.GetAll()
                          join dept in departmentRepository.GetAll()
                          on request.DepartmentId equals dept.Id
                          into groupRequestDept
                          from g1 in groupRequestDept.DefaultIfEmpty()

                          join category in categoryDataRepository.GetAll()
                          on request.PositionId equals category.Id
                          into groupRequestPosition
                          from g2 in groupRequestPosition.DefaultIfEmpty()
                          where request.Id == id
                          select new RecruitmentRequestDTO()
                          {
                              Id = request.Id,
                              IsTemplate = request.IsTemplate,
                              PositionName = request.IsTemplate == true ? string.Empty : g2.Name,
                              DepartmentName = request.IsTemplate == true ? string.Empty : g1.Name,
                              EstimateQuantity = request.EstimateQuantity
                          }).FirstOrDefault();
            return result;
        }

        public PageListResultBO<RecruitmentRequestDTO> GetDataByPage(RecruitmentRequestSearchDTO searchParams, int pageIndex = 1, int pageSize = 20)
        {
            var queryResult = (from request in this.recruitmentRequestRepository.GetAllAsQueryable()
                               join category in this.categoryDataRepository.GetAllAsQueryable()
                               on request.PositionId equals category.Id
                               into groupRequestPosition
                               from g1 in groupRequestPosition.DefaultIfEmpty()

                               join department in this.departmentRepository.GetAllAsQueryable()
                               on request.DepartmentId equals department.Id
                               into groupRequestDepartment
                               from g2 in groupRequestDepartment.DefaultIfEmpty()
                               select new RecruitmentRequestDTO()
                               {
                                   Id = request.Id,
                                   Title = request.Title,
                                   DepartmentId = request.DepartmentId,
                                   PositionId = request.PositionId,
                                   PositionName = g1.Name,
                                   DepartmentName = g2.Name,
                                   EstimateQuantity = request.EstimateQuantity,
                                   Status = request.Status,
                                   UntilDate = request.UntilDate,
                                   IsTemplate = request.IsTemplate,
                                   SkillGroups = request.SkillGroups
                               });
            if (searchParams != null)
            {
                if (!string.IsNullOrEmpty(searchParams.QueryTitle))
                {
                    searchParams.QueryTitle = searchParams.QueryTitle.Trim().ToLower();
                    queryResult = queryResult.Where(x => x.Title.Trim().ToLower().Contains(searchParams.QueryTitle));

                }
                if (searchParams.QueryDepartmentId != null)
                {
                    queryResult = queryResult.Where(x => x.DepartmentId == searchParams.QueryDepartmentId.Value);
                }

                if (!string.IsNullOrEmpty(searchParams.QueryUntilDateFrom))
                {
                    var queryDate = searchParams.QueryUntilDateFrom.ToStartDay();
                    if (queryDate != null)
                    {
                        queryResult = queryResult.Where(x => x.UntilDate >= queryDate);
                    }
                }

                if (!string.IsNullOrEmpty(searchParams.QueryUntileDateTo))
                {
                    var queryDate = searchParams.QueryUntileDateTo.ToEndDay();
                    if (queryDate != null)
                    {
                        queryResult = queryResult.Where(x => x.UntilDate <= queryDate);
                    }
                }

                if (!string.IsNullOrEmpty(searchParams.QueryPositions))
                {
                    List<int> positionIds = searchParams.QueryPositions.ToListNumber<int>(',').ToList();
                    queryResult = queryResult.Where(x => positionIds.Contains(x.PositionId));
                }

                if (searchParams.QueryStatus != null)
                {
                    queryResult = queryResult.Where(x => x.Status == searchParams.QueryStatus);
                }

                if (searchParams.QueryQuantityFrom != null)
                {
                    queryResult = queryResult.Where(x => x.EstimateQuantity >= searchParams.QueryQuantityFrom);
                }

                if (searchParams.QueryQuantityTo != null)
                {
                    queryResult = queryResult.Where(x => x.EstimateQuantity <= searchParams.QueryQuantityTo);
                }

                if (searchParams.QueryTemplate == true)
                {
                    queryResult = queryResult.Where(x => x.IsTemplate == true);
                }
                else
                {
                    queryResult = queryResult.Where(x => x.IsTemplate != true);
                }
                if (!string.IsNullOrEmpty(searchParams.sortQuery))
                {
                    queryResult = queryResult.OrderBy(searchParams.sortQuery);
                }
                else
                {
                    queryResult = queryResult.OrderBy(x => x.UntilDate)
                    .ThenByDescending(x => x.Title);
                }
            }
            else
            {
                queryResult = queryResult.OrderBy(x => x.UntilDate)
                    .ThenByDescending(x => x.Title);
            }

            var result = new PageListResultBO<RecruitmentRequestDTO>();
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

                if (result.ListItem.Any())
                {
                    var dataGroupSkills = recruitmentSkillRepository.GetAll();
                    result.ListItem.ForEach(x =>
                    {
                        var ids = x.SkillGroups.ToListLong(',');
                        x.DataGroupSkills = dataGroupSkills.Where(y => ids.Contains(y.Id));
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách yêu cầu mới để thêm vào đợt tuyển dụng
        /// </summary>
        /// <returns></returns>
        public List<RecruitmentRequestDTO> GetRecruitmentRequestsNew()
        {
            var DsYeuCauTuyenDung = recruitmentRequestRepository.GetAllAsQueryable().Where(x => x.Status != YeuCauTuyenDungTrangThaiConst.HuyBo && x.Status != YeuCauTuyenDungTrangThaiConst.DaDong && x.Status != YeuCauTuyenDungTrangThaiConst.DangTuyen).OrderByDescending(x => x.Id).ToList();
            return _mapper.Map<List<RecruitmentRequestDTO>>(DsYeuCauTuyenDung);
        }
    }
}
