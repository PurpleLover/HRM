using log4net;
using QLNS.Model.IdentityEntities;
using QLNS.Repository;
using QLNS.Repository.AppUserRepository;
using QLNS.Service.AppUserService.Dto;
using QLNS.Service.Common;
using System.Linq.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace QLNS.Service.AppUserService
{
    public class AppUserService : EntityService<AppUser>, IAppUserService
    {
        IUnitOfWork _unitOfWork;
        IAppUserRepository _appUserRepository;
        ILog _loger;
        public AppUserService(IUnitOfWork unitOfWork, IAppUserRepository appUserRepository, ILog loger)
            : base(unitOfWork, appUserRepository)
        {
            _unitOfWork = unitOfWork;
            _appUserRepository = appUserRepository;
            _loger = loger;
            
        }

        public PageListResultBO<AppUser> GetDaTaByPage(AppUserSearchDto searchModel, int pageIndex = 1, int pageSize = 20)
        {
            var query = _appUserRepository.GetAllAsQueryable();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.UserNameFilter))
                {
                    query = query.Where(x => x.UserName.Contains(searchModel.UserNameFilter));
                }

                if (!string.IsNullOrEmpty(searchModel.FullNameFilter))
                {
                    query = query.Where(x => x.FullName.Contains(searchModel.FullNameFilter));
                }
                if (!string.IsNullOrEmpty(searchModel.EmailFilter))
                {
                    query = query.Where(x => x.Email.Contains(searchModel.EmailFilter));
                }
                if (!string.IsNullOrEmpty(searchModel.AddressFilter))
                {
                    query = query.Where(x => x.Address.Contains(searchModel.AddressFilter));
                }

                if (!string.IsNullOrEmpty(searchModel.sortQuery))
                {
                    query = query.OrderBy(searchModel.sortQuery);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }
            var resultmodel = new PageListResultBO<AppUser>();
            if (pageSize == -1)
            {
                var dataPageList = query.ToList();
                resultmodel.Count = dataPageList.Count;
                resultmodel.TotalPage = 1;
                resultmodel.ListItem = dataPageList;
            }
            else
            {
                var dataPageList = query.ToPagedList(pageIndex, pageSize);
                resultmodel.Count = dataPageList.TotalItemCount;
                resultmodel.TotalPage = dataPageList.PageCount;
                resultmodel.ListItem = dataPageList.ToList();
            }
            return resultmodel;
        }

        public AppUser GetById(long id)
        {
            return _appUserRepository.GetById(id);
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của Tài khoản trên hệ thống
        /// </summary>
        /// <returns>
        /// true: Tồn tại
        /// 
        /// </returns>
        public bool CheckExistUserName(string userName, long? id=null)
        {
            return _appUserRepository.GetAll().Where(x => x.UserName != null && x.UserName.Equals(userName) && (id.HasValue ? x.Id != id : true)).Any();
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của Email trên hệ thống
        /// </summary>
        /// <returns>
        /// true: Tồn tại
        /// 
        /// </returns>
        public bool CheckExistEmail(string email, long? id = null)
        {
            return _appUserRepository.GetAll().Where(x => x.Email!=null && x.Email.ToLower().Equals(email.ToLower()) && (id.HasValue ? x.Id != id : true)).Any();
        }

    }
}
