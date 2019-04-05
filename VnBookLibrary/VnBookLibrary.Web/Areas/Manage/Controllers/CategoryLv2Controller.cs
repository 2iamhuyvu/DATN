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
    public class CategoryLv2Controller : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public CategoryLv2Controller()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        public ActionResult Create()
        {
            ViewBag.CategoryLv1Id = new SelectList(UoW.CategoryLv1Repository.GetAll(), "CategoryLv1Id", "CategoryLv1Name");
            return View();
        }
        [HasRole(RoleCode = "CREATE_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryLv2 categoryLv2)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryLv2Repository.Insert(categoryLv2);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Thêm danh mục cấp 2 thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
            }
            ViewBag.CategoryLv1Id = new SelectList(UoW.CategoryLv1Repository.GetAll(), "CategoryLv1Id", "CategoryLv1Name");
            return View(categoryLv2);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            CategoryLv2 categoryLv2 = UoW.CategoryLv2Repository.Find(id);
            if (categoryLv2 == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            ViewBag.CategoryLv1Id = new SelectList(UoW.CategoryLv1Repository.GetAll(), "CategoryLv1Id", "CategoryLv1Name", categoryLv2.CategoryLv1Id);
            return View(categoryLv2);
        }
        [HasRole(RoleCode = "EDIT_CATEGORY")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryLv2 categoryLv2)
        {
            if (ModelState.IsValid)
            {
                UoW.CategoryLv2Repository.Update(categoryLv2);
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Sửa danh mục cấp 2 thành công!",
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
                if (UoW.CategoryLv2Repository.Delete(id) > 0)
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
            return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
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
