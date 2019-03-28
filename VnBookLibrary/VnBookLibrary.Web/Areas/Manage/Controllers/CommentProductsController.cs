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

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    public class CommentProductsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        // GET: Manage/CommentProducts
        public ActionResult Index()
        {
            var commentProducts = db.CommentProducts.Include(c => c.Customer).Include(c => c.Product);
            return View(commentProducts.ToList());
        }

        // GET: Manage/CommentProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            return View(commentProduct);
        }

        // GET: Manage/CommentProducts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: Manage/CommentProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,CustomerId,Comments,CommentDate,AlowDisplay")] CommentProduct commentProduct)
        {
            if (ModelState.IsValid)
            {
                db.CommentProducts.Add(commentProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", commentProduct.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", commentProduct.ProductId);
            return View(commentProduct);
        }

        // GET: Manage/CommentProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", commentProduct.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", commentProduct.ProductId);
            return View(commentProduct);
        }

        // POST: Manage/CommentProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,CustomerId,Comments,CommentDate,AlowDisplay")] CommentProduct commentProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", commentProduct.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", commentProduct.ProductId);
            return View(commentProduct);
        }

        // GET: Manage/CommentProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return HttpNotFound();
            }
            return View(commentProduct);
        }

        // POST: Manage/CommentProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            db.CommentProducts.Remove(commentProduct);
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
