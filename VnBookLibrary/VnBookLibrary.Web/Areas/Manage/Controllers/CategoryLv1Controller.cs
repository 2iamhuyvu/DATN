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
    public class CategoryLv1Controller : Controller
    {
        private VnBookLibraryDbContext db;
        private  UnitOfWork UoW;
        public CategoryLv1Controller()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        // GET: Manage/CategoryLv1/Create
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
        public ActionResult Create(CategoryLv1 categoryLv1)
        {
            if (ManageSession.HasRole("CREATE_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    categoryLv1.OrderDisplay = categoryLv1.OrderDisplay ?? 1;
                    UoW.CategoryLv1Repository.Insert(categoryLv1);
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thêm danh mục cấp 1 thành công!",
                    };
                    return RedirectToAction("Index", "BookCategories", new { Area = "Manage" ,displayCategory=1});
                }
                return View(categoryLv1);
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
                CategoryLv1 categoryLv1 = UoW.CategoryLv1Repository.Find(id);
                if (categoryLv1 == null)
                {
                    return HttpNotFound();
                }
                return View(categoryLv1);
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
        public ActionResult Edit(CategoryLv1 categoryLv1)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    categoryLv1.OrderDisplay = categoryLv1.OrderDisplay ?? 1;
                    UoW.CategoryLv1Repository.Insert(categoryLv1);
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa danh mục cấp 1 thành công!",
                    };                   
                }
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
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
                UoW.CategoryLv1Repository.Delete(id);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa danh mục thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
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
