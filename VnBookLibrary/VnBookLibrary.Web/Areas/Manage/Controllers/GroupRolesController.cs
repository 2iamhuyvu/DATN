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
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class GroupRolesController : Controller
    {
        private VnBookLibraryDbContext db;
        private GroupRoleRepository _groupRoleRepository;
        public GroupRolesController()
        {
            db = new VnBookLibraryDbContext();
            _groupRoleRepository = new GroupRoleRepository(db);
        }
        [HasRole]
        public ActionResult Index()
        {
            var list = db.GroupRoles.ToList();
            return View(db.GroupRoles.ToList());
        }
        [HasRole]        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRole groupRole = db.GroupRoles.Find(id);
            if (groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRole);
        }
        [HasRole]        
        public ActionResult Create()
        {
            return View();
        }

        [HasRole]
        [HttpPost]        
        public ActionResult Create(GroupRole groupRole)
        {
            if (ModelState.IsValid)
            {
                db.GroupRoles.Add(groupRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupRole);
        }
        [HasRole]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRole groupRole = db.GroupRoles.Find(id);
            if (groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRole);
        }
        [HasRole]
        [HttpPost]        
        public ActionResult Edit(GroupRole groupRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupRole);
        }
        [HasRole]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupRole groupRole = db.GroupRoles.Find(id);
            if (groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRole);
        }
        [HasRole]
        [HttpPost, ActionName("Delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
            GroupRole groupRole = db.GroupRoles.Find(id);
            db.GroupRoles.Remove(groupRole);
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
