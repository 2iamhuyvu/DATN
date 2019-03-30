using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class RolesController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();
        private GroupRoleRepository _groupRoleRepository;
        private RoleRepository _roleRepository;
        // GET: Manage/Roles        
        public ActionResult Index()
        {
            if (ManageSession.HasRole("VIEW_ROLE")||true)
            {
                ViewData["ListGroupRole"] = db.GroupRoles.ToList();
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền xem nhóm quyền!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
