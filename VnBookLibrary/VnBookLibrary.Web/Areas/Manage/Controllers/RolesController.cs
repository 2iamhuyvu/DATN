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
        public ActionResult Index1()
        {
            return View(db.Roles.ToList().OrderBy(x=>x.GroupRoleId).ToList());
        }
            public ActionResult CreateGroup()
        {
            if (ManageSession.HasRole("CREATE_GROUPROLE")||true)
            {
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm nhóm quyền!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }
        [HttpPost]
        public ActionResult CreateGroup(GroupRole groupRole)
        {
            if (ManageSession.HasRole("CREATE_GROUPROLE")||true)
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
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm nhóm quyền!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }
        public ActionResult EditGroup(int? idGroup)
        {
            if (ManageSession.HasRole("CREATE_GROUPROLE")||true)
            {
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm nhóm quyền!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }
        [HttpPost]
        public ActionResult EditGroup(GroupRole groupRole)
        {
            if (ManageSession.HasRole("CREATE_GROUPROLE")||true)
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
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm nhóm quyền!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }
        // GET: Manage/Roles/Create
        public ActionResult Create()
        {
            ViewBag.GroupRoleId = new SelectList(db.GroupRoles, "GroupRoleId", "GroupRoleName");
            return View();
        }

        // POST: Manage/Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleCode,RoleName,Description,URL,GroupRoleId")] Role role)
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

        // GET: Manage/Roles/Edit/5
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

        // POST: Manage/Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleCode,RoleName,Description,URL,GroupRoleId")] Role role)
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

        // GET: Manage/Roles/Delete/5
        public ActionResult Delete(string id)
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
            return View(role);
        }

        // POST: Manage/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
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
