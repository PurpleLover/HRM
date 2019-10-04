using AutoMapper;
using log4net;
using PagedList;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.TD_CauHinhSangLocUngVienRepository;
using QLNS.Repository.TD_QuanLyMauTestRepository;
using QLNS.Service.Common;
using QLNS.Service.TD_CauHinhSangLocUngVienService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.TD_CauHinhSangLocUngVienService
{
    public class TD_CauHinhSangLocUngVienService : EntityService<TD_CauHinhSangLocUngVien>, ITD_CauHinhSangLocUngVienService
    {
        IUnitOfWork _unitOfWork;
        ITD_CauHinhSangLocUngVienRepository _cauHinhSangLocUngVienRepository;
        ITD_QuanLyMauTestRepository _quanLyMauTestRepository;
        ILog _loger;
        IMapper _mapper;

        public TD_CauHinhSangLocUngVienService(IUnitOfWork unitOfWork,
            ITD_CauHinhSangLocUngVienRepository cauHinhSangLocUngVienRepository,
            ITD_QuanLyMauTestRepository quanLyMauTestRepository,
            ILog loger, IMapper mapper)
            : base(unitOfWork, cauHinhSangLocUngVienRepository)
        {
            _unitOfWork = unitOfWork;
            _cauHinhSangLocUngVienRepository = cauHinhSangLocUngVienRepository;
            _quanLyMauTestRepository = quanLyMauTestRepository;
            _loger = loger;
            _loger.Info("Khởi tạo TD_CauHinhSangLocUngVienService service");
            _mapper = mapper;
        }
        public List<CauHinhSangLocUngVienDTO> GetDataByDotTuyenDungId(long dotTuyenDungId = 0)
        {
            var query = (from tbl in _cauHinhSangLocUngVienRepository.GetAllAsQueryable()
                         where tbl.DotTuyenDungId == dotTuyenDungId
                         join qlmt in _quanLyMauTestRepository.GetAllAsQueryable()
                         on tbl.TestId equals qlmt.Id
                         into group1
                         from g1 in group1.DefaultIfEmpty()
                         select new CauHinhSangLocUngVienDTO
                         {
                             Id = tbl.Id,
                             DotTuyenDungId = tbl.DotTuyenDungId,
                             Type = tbl.Type,
                             Note = tbl.Note,
                             Priority = tbl.Priority,
                             Title = tbl.Title,
                             FileDirectory = g1.FileDirectory,
                             FileName = g1.FileName,
                             TestId = tbl.TestId,
                             TestApprovedLimit = tbl.TestApprovedLimit,
                             InterviewDate = tbl.InterviewDate,
                             InterviewerId = tbl.InterviewerId,
                             InterviewerName = tbl.InterviewerName
                         });
            return query.OrderBy(x => x.Priority).ToList();
        }
    }
}
