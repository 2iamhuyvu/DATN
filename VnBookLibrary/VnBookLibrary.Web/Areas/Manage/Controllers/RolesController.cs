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
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public RolesController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole]
        public ActionResult Index()
        {
            ViewData["ListGroupRole"] = db.GroupRoles.ToList();
            return View();
        }
        [HasRole]
        public ActionResult EditGroup(int? idGroup)
        {
            return View();
        }
        [HasRole]
        [HttpPost]
        public ActionResult EditGroup(GroupRole groupRole)
        {
            if (groupRole.GroupRoleName == "")
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Không được để trống tên nhóm!",
                };
            }
            else
            {
                db.GroupRoles.Add(groupRole);
                db.SaveChanges();
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Thêm thành công!",
                };
                return RedirectToAction("Index");
            }
            return View(groupRole);
        }
        [HasRole]
        public ActionResult Create()
        {
            ViewBag.GroupRoleId = new SelectList(db.GroupRoles, "GroupRoleId", "GroupRoleName");
            return View();
        }
        [HasRole]
        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupRoleId = new SelectList(db.GroupRoles, "GroupRoleId", "GroupRoleName", role.GroupRoleId);
            return View(role);
        }
        [HasRole]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupRoleId = new SelectList(db.GroupRoles, "GroupRoleId", "GroupRoleName", role.GroupRoleId);
            return View(role);
        }
        [HasRole]
        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupRoleId = new SelectList(db.GroupRoles, "GroupRoleId", "GroupRoleName", role.GroupRoleId);
            return View(role);
        }
        [HasRole]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (UoW.RoleRepository.Delete(id) > 0)
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa thành công!",
                };
                return RedirectToAction("Index");
            }
            return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
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
