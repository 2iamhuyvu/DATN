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
    public class CategoryByPublishersController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public CategoryByPublishersController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryByPublisher categoryByPublisher)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryByPublisherRepository.Insert(categoryByPublisher);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Thêm danh mục NXB - NPH thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
            }
            return View(categoryByPublisher);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryByPublisher categoryByPublisher = UoW.CategoryByPublisherRepository.Find(id);
            if (categoryByPublisher == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(categoryByPublisher);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryByPublisher categoryByPublisher)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryByPublisherRepository.Update(categoryByPublisher);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Sửa danh mục tác giả thành công!",
                };
                return View();
            }
            return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
        }
        [HasRole(RoleCode = "DELETE_CATEGORY")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            UoW.CategoryByPublisherRepository.Delete(id);
            TempData["Notify"] = new JsonResultBO()
            {
                Status = true,
                Message = "Xóa danh mục thành công!",
            };
            return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
        }
        [NonAction]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryByPublisher categoryByPublisher = db.CategoryByPublishers.Find(id);
            if (categoryByPublisher == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(categoryByPublisher);
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
