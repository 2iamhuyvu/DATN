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
    public class CategoryByAuthorsController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public CategoryByAuthorsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        public ActionResult Create()
        {
            if (ManageSession.HasRole("CREATE_CATEGORY"))
            {
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền tạo danh mục!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
            }
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryByAuthor categoryByAuthor)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryByAuthorRepository.Insert(categoryByAuthor);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Thêm danh mục tác giả thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 2 });
            }
            return View(categoryByAuthor);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryByAuthor categoryByAuthor = UoW.CategoryByAuthorRepository.Find(id);
            if (categoryByAuthor == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(categoryByAuthor);
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryByAuthor categoryByAuthor)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryByAuthorRepository.Update(categoryByAuthor);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Sửa danh mục tác giả thành công!",
                };
                return View();
            }
            return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 2 });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (UoW.CategoryByAuthorRepository.Delete(id) > 0)
            {

                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa danh mục thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 2 });
            }
            return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
        }
        [NonAction]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryByAuthor categoryByAuthor = UoW.CategoryByAuthorRepository.Find(id);
            if (categoryByAuthor == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(categoryByAuthor);
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
