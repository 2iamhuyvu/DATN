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
        private VnBookLibraryDbContext db;
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
        [HasRole(RoleCode = "VIEW_CUSTOMER")]
        public ActionResult _TableCustomer()
        {
            return PartialView(db.Customers.ToList());
        }
        [HasRole(RoleCode = "BLOCK_CUSTOMER")]
        [HttpPost]
        public ActionResult BlockCustomer(int id, bool IsBlock)
        {
            if (ManageSession.HasRole("BLOCK_CUSTOMER"))
            {
                var ctm = UoW.CustomerRepository.Find(id);
                if (ctm != null)
                {
                    ctm.IsBlock = IsBlock;
                    ctm.RePassword = ctm.Password;
                    UoW.CustomerRepository.Update(ctm);
                    return Json(new JsonResultBO(true) { Message = IsBlock ? "Đã khóa khách hàng" : "Đã mở khóa khách hàng" });
                }
                return Json(new JsonResultBO(false) { Message = "Không tồn tại khách hàng này" });
            }
            return Json(new JsonResultBO(false) { Message = "Bạn không có quyền này" });
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
