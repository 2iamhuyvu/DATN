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
        private UnitOfWork UoW;
        public CategoryLv1Controller()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        public ActionResult Create()
        {
            return View();
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryLv1 categoryLv1)
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
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
            }
            return View(categoryLv1);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryLv1 categoryLv1 = UoW.CategoryLv1Repository.Find(id);
            if (categoryLv1 == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            return View(categoryLv1);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryLv1 categoryLv1)
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
        [HasRole(RoleCode = "DELETE_CATEGORY")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_CATEGORY"))
            {
                if (UoW.CategoryLv1Repository.Delete(id) > 0)
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Xóa danh mục thành công!",
                    };
                    return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
                }
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            TempData["Notify"] = new JsonResultBO()
            {
                Status = false,
                Message = "Bạn không có quyền này!",
            };
            return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
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
