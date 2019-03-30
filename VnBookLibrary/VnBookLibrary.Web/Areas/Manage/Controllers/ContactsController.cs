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
    public class ContactsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        // GET: Manage/Contacts
        public ActionResult Index()
        {
            var contacts = db.Contacts.Include(c => c.Customer).Include(c => c.Employee);
            return View(contacts.ToList());
        }

        // GET: Manage/Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Manage/Contacts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName");
            return View();
        }

        // POST: Manage/Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactId,CustomerEmail,CustomerPhone,ContactContent,ContactSendTime,CustomerId,IsReply,EmployeeId,ReplyContent")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", contact.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", contact.EmployeeId);
            return View(contact);
        }

        // GET: Manage/Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", contact.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", contact.EmployeeId);
            return View(contact);
        }

        // POST: Manage/Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactId,CustomerEmail,CustomerPhone,ContactContent,ContactSendTime,CustomerId,IsReply,EmployeeId,ReplyContent")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", contact.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", contact.EmployeeId);
            return View(contact);
        }

        // GET: Manage/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Manage/Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
