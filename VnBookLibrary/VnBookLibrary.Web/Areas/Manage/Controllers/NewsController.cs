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
    public class NewsController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public NewsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }

        public ActionResult Index()
        {
            return View(UoW.NewsRepository.GetAll());
        }        

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(News news)
        {
            news.PostDate = DateTime.Now;
            UoW.NewsRepository.Insert(news);
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
            News news = UoW.NewsRepository.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
        [HttpPost]
        public ActionResult Edit(News news)
        {
            UoW.NewsRepository.Update(news);
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
            UoW.NewsRepository.Delete(id);
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
                UoW.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
