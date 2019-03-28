using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class ManageHomeController : Controller
    {
        // GET: Manage/Home
        public ActionResult Index()
        {
            return View();
        }        
    }
}
