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
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class SaleEventsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        // GET: Manage/SaleEvents
        public ActionResult Index()
        {
            return View(db.SaleEvents.ToList());
        }

        // GET: Manage/SaleEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleEvent saleEvent = db.SaleEvents.Find(id);
            if (saleEvent == null)
            {
                return HttpNotFound();
            }
            return View(saleEvent);
        }

        // GET: Manage/SaleEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manage/SaleEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleEventId,SaleEventName,Descripton,DateStart,DateEnd,Percent")] SaleEvent saleEvent)
        {
            if (ModelState.IsValid)
            {
                db.SaleEvents.Add(saleEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saleEvent);
        }

        // GET: Manage/SaleEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleEvent saleEvent = db.SaleEvents.Find(id);
            if (saleEvent == null)
            {
                return HttpNotFound();
            }
            return View(saleEvent);
        }

        // POST: Manage/SaleEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleEventId,SaleEventName,Descripton,DateStart,DateEnd,Percent")] SaleEvent saleEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saleEvent);
        }

        // GET: Manage/SaleEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleEvent saleEvent = db.SaleEvents.Find(id);
            if (saleEvent == null)
            {
                return HttpNotFound();
            }
            return View(saleEvent);
        }

        // POST: Manage/SaleEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleEvent saleEvent = db.SaleEvents.Find(id);
            db.SaleEvents.Remove(saleEvent);
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
