using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Repository.Commons;

namespace VnBookLibrary.Web.Areas.Manage.Customizes
{
    public class AuthorizeManage : AuthorizeAttribute
    {       
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var session = httpContext.Session[Constants.MANAGE_SESSION];
            if (session == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            bool allowAnonymous = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!allowAnonymous)
            {
                var url = filterContext.HttpContext.Request.Url.AbsolutePath;
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary()
                    {
                        {"Area","Manage" },
                        {"Controller","Login"},
                        {"Action","Index"},
                        {"returnURL",url }
                    });
            }
            else return;            
        }
    }
}