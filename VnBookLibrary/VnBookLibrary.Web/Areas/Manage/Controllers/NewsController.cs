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
        [HasRole(RoleCode = "CREATE_NEWS")]
        public ActionResult Create()
        {
            return View();
        }
        [HasRole(RoleCode = "CREATE_NEWS")]
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

        [HasRole(RoleCode = "EDIT_NEWS")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            News news = UoW.NewsRepository.Find(id);
            if (news == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(news);
        }


        [HasRole(RoleCode = "EDIT_NEWS")]
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
        [HasRole(RoleCode = "DELETE_NEWS")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_NEWS"))
            {
                if (UoW.NewsRepository.Delete(id) > 0)
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Xóa tin tức thành công!",
                    };
                    return RedirectToAction("Index");
                }
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            TempData["Notify"] = new JsonResultBO()
            {
                Status = false,
                Message = "Bạn không có quyền này!",
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
