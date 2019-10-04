using log4net;
using Microsoft.AspNet.Identity;
using QLNS.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.Web.Filters
{
    public class BaseController : Controller
    {
        ILog _loger;
        protected long? CurrentUserId = null;
        public BaseController()
        {
            _loger = LogManager.GetLogger("RollingLogFileAppender");
            
            
            if (System.Web.HttpContext.Current.User != null)
            {
                CurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId<long>();
            }
        }
        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = RedirectToAction("Login", "Account", new { Area=""});    
            //}

            base.OnAuthentication(filterContext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
                if (!skipAuthorization)
                {

                    if (filterContext.HttpContext.Session.IsNewSession)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {

                            if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(JsonResult))
                            {
                                var rs = new JsonResultBO(false);
                                rs.Message = "Phiên làm việc của bạn đã hết";
                                filterContext.Result = Json(rs);
                            }
                            else if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(PartialViewResult))
                            {
                                filterContext.Result =
                                RedirectToAction("TimeOutSession", "Error", new { area = "" });
                            }

                        }
                        else
                        {
                            filterContext.Result =
                           RedirectToAction("login", "account", new { area = "" });
                        }

                        return;

                    }
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            _loger.Error("Lỗi hệ thống", filterContext.Exception);
            TempData["filterContext"] = filterContext;
            //filterContext.ExceptionHandled = true;

            //// Redirect on error:
            //filterContext.Result = RedirectToAction("Index", "Errors", filterContext.Exception);

            // OR set the result without redirection:
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Errors/Index.cshtml"
            //};
        }
    }
}