using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Web.Areas.Manage.Models;

namespace VnBookLibrary.Web.Areas.Manage
{
    public class HasRole : ActionFilterAttribute
    {
        public string RoleCode { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (ManageSessionModel)filterContext.HttpContext.Session[Constants.MANAGE_SESSION];
            var roles = session.RoleCodes;
            Employee employee = session.SessionEmployee;
            if (employee.EmployeeType.IsAdministrator == true && RoleCode == null)
            {
                return;
            }
            bool check = roles.Contains(RoleCode);
            if (!check)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(JsonResult))
                    {
                        var rs = new JsonResultBO(false);
                        rs.Message = "Bạn không có quyền này!";
                        filterContext.Result = new JsonResult
                        {
                            Data = rs,
                        };
                    }
                    else if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(PartialViewResult))
                    {
                        filterContext.Result = new PartialViewResult
                        {
                            ViewName = "~/Areas/Manage/Views/Shared/_UnAuthorizePartial.cshtml",
                        };
                    }
                }
                else
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Areas/Manage/Views/Shared/_UnAuthorize.cshtml"
                    };
                }
            }
            return;
        }
    }
}