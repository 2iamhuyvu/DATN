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
    public class CommentProductsController : Controller
    {
        private VnBookLibraryDbContext db ;
        UnitOfWork Uow;
        public CommentProductsController()
        {
            db = new VnBookLibraryDbContext();
            Uow = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "VIEW_COMMENT")]
        public ActionResult Index()
        {            
            return View(Uow.CommentProductRepository.GetAll());
        }
        [HasRole(RoleCode = "EDIT_COMMENT")]
        [HttpPost]
        public ActionResult DisplayComment(int id,bool isDisplay)
        {
            var cmt=Uow.CommentProductRepository.Find(id);
            if (cmt != null)
                cmt.AlowDisplay = isDisplay;
            Uow.CommentProductRepository.Update(cmt);
            return RedirectToAction("Index");
        }
        [HasRole(RoleCode = "DELETE_COMMENT")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Uow.CommentProductRepository.Delete(id) > 0)
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa thành công!",
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
