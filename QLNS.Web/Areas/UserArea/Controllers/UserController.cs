using AutoMapper;
using CommonHelper.String;
using CommonHelper.Upload;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using QLNS.Model.IdentityEntities;
using QLNS.Service.AppUserService;
using QLNS.Service.AppUserService.Dto;
using QLNS.Service.Common;
using QLNS.Service.Constant;
using QLNS.Service.RoleService;
using QLNS.Service.UserRoleService;
using QLNS.Web.Areas.UserArea.Models;
using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace QLNS.Web.Areas.UserArea.Controllers
{
    public class UserController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly ILog _Ilog;
        private readonly IMapper _mapper;
        public UserController(IAppUserService appUserService, ILog Ilog,
            IRoleService roleService,
            IUserRoleService userRoleService,
            IMapper mapper
            )
        {
            _userRoleService = userRoleService;
            _appUserService = appUserService;
            _Ilog = Ilog;
            _roleService = roleService;
            _mapper = mapper;
        }
        // GET: UserArea/User
        [Authorize]
        public ActionResult Index()
        {
            var userData = _appUserService.GetDaTaByPage(null);
            return View(userData);
        }

        [HttpPost]
        public JsonResult getData(int indexPage, string sortQuery, int pageSize)
        {
            var searchModel = SessionManager.GetValue("UserPageSearchModel") as AppUserSearchDto;
            if (!string.IsNullOrEmpty(sortQuery))
            {
                if (searchModel == null)
                {
                    searchModel = new AppUserSearchDto();
                }
                searchModel.sortQuery = sortQuery;
                if (pageSize > 0)
                {
                    searchModel.pageSize = pageSize;
                }
                SessionManager.SetValue("UserPageSearchModel", searchModel);
            }
            var data = _appUserService.GetDaTaByPage(searchModel, indexPage, pageSize);
            return Json(data);
        }
        public PartialViewResult Create()
        {
            var myModel = new CreateVM();

            return PartialView("_CreatePartial", myModel);
        }

        public PartialViewResult Edit(long id)
        {
            var myModel = new EditVM();
            var user = _appUserService.GetById(id);
            if (user == null)
            {
                throw new HttpException(404, "Không tìm thấy thông tin");
            }
            myModel = new EditVM()
            {
                Id = user.Id,
                FullName = user.FullName,
                Address = user.Address,
                BirthDay = user.BirthDay,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return PartialView("_EditPartial", myModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditVM model)
        {
            var result = new JsonResultBO(true);
            try
            {
                if (ModelState.IsValid)
                {

                    var user = _appUserService.GetById(model.Id);
                    if (user == null)
                    {
                        throw new Exception("Không tìm thấy thông tin");
                    }

                    if (!string.IsNullOrEmpty(model.Email) && _appUserService.CheckExistEmail(model.Email, user.Id))
                    {
                        throw new Exception(string.Format("Email {0} đã được sửa dụng", model.Email));
                    }
                    if (user != null)
                    {
                        user.FullName = model.FullName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.BirthDay = model.BirthDay;
                        user.Address = model.Address;
                        user.Gender = model.Gender;
                        user.Email = model.Email;
                        _appUserService.Update(user);
                    }


                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "Không cập nhật được";
                _Ilog.Error("Lỗi cập nhật thông tin User", ex);
            }
            return Json(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CreateVM model)
        {
            var result = new JsonResultBO(true, "Tạo tài khoản thành công");
            try
            {
                if (ModelState.IsValid)
                {
                    if (_appUserService.CheckExistUserName(model.UserName))
                    {
                        throw new Exception(string.Format("Tài khoản {0} đã tồn tại", model.UserName));
                    }
                    if (_appUserService.CheckExistEmail(model.Email))
                    {
                        throw new Exception(string.Format("Email {0} đã được sửa dụng", model.Email));
                    }
                    var user = new AppUser();
                    user.UserName = model.UserName;
                    user.FullName = model.FullName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.BirthDay = model.BirthDay;
                    user.Address = model.Address;
                    user.Gender = model.Gender;
                    user.Email = model.Email;
                    user.Avatar = "images/avatars/profile-pic.jpg";
                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    //Kiểm tra thông tin tài khoản
                    var resultUser = UserManager.CreateAsync(user, UserConst.DefaultPassword).Result;
                    if (!resultUser.Succeeded)
                    {
                        throw new Exception(getErrorString(resultUser));
                    }

                }

            }
            catch (Exception ex)
            {
                result.MessageFail(ex.Message);
                _Ilog.Error("Lỗi tạo mới tài khoản", ex);
            }
            return Json(result);
        }
        private string getErrorString(IdentityResult identityResult)
        {
            var strMessage = string.Empty;
            foreach (var item in identityResult.Errors)
            {
                strMessage += item;
            }
            return strMessage;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult searchData(AppUserSearchDto form)
        {
            var searchModel = SessionManager.GetValue("UserPageSearchModel") as AppUserSearchDto;

            if (searchModel == null)
            {
                searchModel = new AppUserSearchDto();
                searchModel.pageSize = 20;
            }

            searchModel.UserNameFilter = form.UserNameFilter;
            searchModel.AddressFilter = form.AddressFilter;
            searchModel.EmailFilter = form.EmailFilter;
            searchModel.FullNameFilter = form.FullNameFilter;
            SessionManager.SetValue("UserPageSearchModel", searchModel);

            var data = _appUserService.GetDaTaByPage(searchModel, 1, searchModel.pageSize);
            return Json(data);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var result = new JsonResultBO(true, "Xóa tài khoản thành công");
            try
            {
                var user = _appUserService.GetById(id);
                if (user == null)
                {
                    throw new Exception("Không tìm thấy thông tin để xóa");
                }
                _appUserService.Delete(user);
            }
            catch (Exception ex)
            {
                result.MessageFail("Không thực hiện được");
                _Ilog.Error("Lỗi khi xóa tài khoản id=" + id, ex);
            }
            return Json(result);
        }

        public ActionResult SetupRole(long id)
        {
            var user = _appUserService.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserInfo = user;
            ViewBag.ListRole = _roleService.GetAll().ToList();
            ViewBag.UserListRole = _userRoleService.GetRoleOfUser(user.Id);
            return PartialView("_SetupRolePartial");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveSetupRole(List<int> ListRoleUser, long userId)
        {
            var result = new JsonResultBO(true, "Thiết lập vai trò thành công");
            var userData = _appUserService.GetById(userId);
            if (userData == null)
            {
                return HttpNotFound();
            }
            var isSuccess=_userRoleService.SaveRole(ListRoleUser, userId);
            if (!isSuccess)
            {
                result.MessageFail("Lỗi khi thiết lập vai trò người dùng");
            }
            return Json(result);
        }
        public ActionResult Detail(int id)
        {
            var model = new DetailVM();
            model.users = _appUserService.GetById(id);
            return View(model);
        }
        public PartialViewResult EditAvatar(long id)
        {
            var model = new DetailVM();
            model.users = _appUserService.GetById(id);
            return PartialView("_EditAvatarPartial", model);
        }
        [HttpPost]
        public ActionResult EditAvatar(FormCollection collection, HttpPostedFileBase File)
        {
            var id = collection["ID"].ToIntOrZero();
            var myModel = _appUserService.GetById(id);
            var result = new JsonResultBO(true);
            try
            {
                if (File != null && File.ContentLength > 0)
                {
                    var resultUpload = UploadProvider.SaveFile(File, null, ".jpg,.png", null, "images/avatars" + myModel.Id.ToString(), HostingEnvironment.MapPath("/assets"));

                    if (resultUpload.status == true)
                    {
                        myModel.Avatar = resultUpload.path;
                    }
                }
                _appUserService.Update(myModel);

            }
            catch
            {
                result.Status = false;
                result.Message = "Không cập nhật được";
            }
            return RedirectToAction("Detail", new {id});
        }
    }
}