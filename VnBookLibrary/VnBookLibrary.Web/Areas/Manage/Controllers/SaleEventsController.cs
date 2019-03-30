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
    public class SaleEventsController : Controller
    {
        private VnBookLibraryDbContext db ;
        private UnitOfWork UoW;
        public SaleEventsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        
        public ActionResult Index()
        {
            return View(db.SaleEvents.ToList());
        }
        
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

        [HasRole(RoleCode = "CREATE_EVENTSALE")]
        public ActionResult Create()
        {
            return View();
        }

        [HasRole(RoleCode = "CREATE_EVENTSALE")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaleEvent saleEvent)
        {
            if (ModelState.IsValid)
            {
                db.SaleEvents.Add(saleEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saleEvent);
        }

        [HasRole(RoleCode = "EDIT_EVENTSALE")]
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

        [HasRole(RoleCode = "EDIT_EVENTSALE")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaleEvent saleEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saleEvent);
        }
        [HasRole(RoleCode = "DELETE_EVENTSALE")]
        [HttpPost]        
        public ActionResult Delete(int id)
        {
            if (UoW.SaleEventRepository.Delete(id) > 0)
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
