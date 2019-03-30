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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryByAuthor categoryByAuthor)
        {
            if (ManageSession.HasRole("CREATE_CATEGORY"))
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

        public ActionResult Edit(int? id)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CategoryByAuthor categoryByAuthor = UoW.CategoryByAuthorRepository.Find(id);
                if (categoryByAuthor == null)
                {
                    return HttpNotFound();
                }
                return View(categoryByAuthor);
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền sửa danh mục!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryByAuthor categoryByAuthor)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
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
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền sửa danh mục!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_CATEGORY"))
            {                
                UoW.CategoryByAuthorRepository.Delete(id);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa danh mục thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 2 });
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền sửa danh mục!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
            }
        }
        [NonAction]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryByAuthor categoryByAuthor = UoW.CategoryByAuthorRepository.Find(id);
            if (categoryByAuthor == null)
            {
                return HttpNotFound();
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
