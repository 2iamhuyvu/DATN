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
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class NewsController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();

        public ActionResult Index()
        {
            return View(db.News.ToList());
        }        

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(News news)
        {
            news.PostDate = DateTime.Now;
            db.News.Add(news);
            db.SaveChanges();
            TempData["Notify"] = new JsonResultBO()
            {
                Status = true,
                Message = "Thêm tin tức thành công!",
            };
            return Json(new JsonResultBO(true) { Message = "Thêm tin tức thành công!" });
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
        [HttpPost]
        public ActionResult Edit(News news)
        {
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Notify"] = new JsonResultBO()
            {
                Status = true,
                Message = "Chỉnh sửa tin tức thành công!",
            };
            return Json(new JsonResultBO(true) { Message = "Chỉnh sửa tin tức thành công!" });
        }
       
        [HttpPost]        
        public ActionResult Delete(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            TempData["Notify"] = new JsonResultBO()
            {
                Status = true,
                Message = "Xóa tin tức thành công!",
            };
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
