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
    public class CommentProductsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        [HasRole(RoleCode = "VIEW_COMMENT")]
        public ActionResult Index()
        {
            var commentProducts = db.CommentProducts.Include(c => c.Customer).Include(c => c.Product);
            return View(commentProducts.ToList());
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
