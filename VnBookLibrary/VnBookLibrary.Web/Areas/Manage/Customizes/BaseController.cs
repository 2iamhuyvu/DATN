using System.Collections.Generic;
using System.Linq;
using System.Web;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Repository.Commons;
using System.Web.Mvc;

namespace VnBookLibrary.Web.Areas.Manage.Customizes
{    
    public class BaseController : Controller
    {        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            if (filterContext.HttpContext.Session != null)
            {
                bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
                if (!skipAuthorization)
                {
                    if (filterContext.HttpContext.Session.IsNewSession || filterContext.HttpContext.Session[Constants.MANAGE_SESSION] == null)
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
                                filterContext.Result = RedirectToAction("SessionEnd", "Login", new { area = "Manage"});
                            }
                        }
                        else
                        {
                            filterContext.Result = RedirectToAction("Index", "Login", new { area = "Manage"});
                        }
                        return;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}