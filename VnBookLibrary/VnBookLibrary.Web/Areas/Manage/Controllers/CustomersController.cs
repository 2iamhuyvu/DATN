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
    public class CustomersController : Controller
    {
        private VnBookLibraryDbContext db ;
        private UnitOfWork UoW;
        public CustomersController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "VIEW_CUSTOMER")]
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
        [HttpPost]
        public ActionResult BlockCustomer(int id,bool IsBlock)
        {
            var ctm = UoW.CustomerRepository.Find(id);
            if (ctm != null)
            {
                ctm.IsBlock = IsBlock;
                ctm.RePassword = ctm.Password;
                UoW.CustomerRepository.Update(ctm);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = IsBlock? "Đã khóa tài khoản!":"Đã mở khóa tài khoản!",
                };
            }
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
