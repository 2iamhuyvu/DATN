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
        // GET: Manage/GroupRoles
        public ActionResult Index()
        {
            var list = db.GroupRoles.ToList();
            return View(db.GroupRoles.ToList());
        }

        // GET: Manage/GroupRoles/Details/5
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

        // GET: Manage/GroupRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manage/GroupRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupRoleId,GroupRoleName,Description")] GroupRole groupRole)
        {
            if (ModelState.IsValid)
            {
                db.GroupRoles.Add(groupRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groupRole);
        }

        // GET: Manage/GroupRoles/Edit/5
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

        // POST: Manage/GroupRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupRoleId,GroupRoleName,Description")] GroupRole groupRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groupRole);
        }

        // GET: Manage/GroupRoles/Delete/5
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

        // POST: Manage/GroupRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
