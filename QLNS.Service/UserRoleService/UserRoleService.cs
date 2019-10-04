using AutoMapper;
using log4net;
using QLNS.Model.Entities;
using QLNS.Repository;
using QLNS.Repository.UserRoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Service.UserRoleService
{
    public class UserRoleService : EntityService<UserRole>, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserRoleService(IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository,
            ILog logger, IMapper mapper) : base(unitOfWork, userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public List<UserRole> GetRoleOfUser(long userId) => _userRoleRepository.FindBy(x => x.UserId == userId).ToList();
        public void AddListRole(List<int> listRoleId, long UserId)
        {
            foreach (var item in listRoleId)
            {
                var userRole = new UserRole()
                {
                    RoleId = item,
                    UserId = UserId
                };
                _userRoleRepository.Add(userRole);
            }
        }

        public bool SaveRole(List<int> listRoleId, long UserId)
        {
            try
            {
                var ListRoleUserDB = _userRoleRepository.GetAllAsQueryable().Where(x => x.UserId == UserId).ToList();
                if (ListRoleUserDB == null || !ListRoleUserDB.Any())
                {
                    if (listRoleId != null && listRoleId.Any())
                    {
                        AddListRole(listRoleId, UserId);
                    }
                }
                else
                {
                    if (listRoleId == null || !listRoleId.Any())
                    {
                        _userRoleRepository.DeleteRange(ListRoleUserDB);
                    }
                    else
                    {
                        var listNew = listRoleId.Where(x => !ListRoleUserDB.Any(a => a.RoleId == x)).ToList();
                        if (listNew != null)
                        {
                            AddListRole(listNew, UserId);
                        }
                        var listDelete = ListRoleUserDB.Where(x => !listRoleId.Any(a => a == x.RoleId)).ToList();
                        if (listDelete != null)
                        {
                            _userRoleRepository.DeleteRange(listDelete);
                        }

                    }
                }
                _userRoleRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Lỗi khi lưu thiết lập role cho user", ex);
                return false;

            }

        }
    }
}
