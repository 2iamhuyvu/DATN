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
    public class CategoryByPublishersController : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();


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
        public ActionResult Create(CategoryByPublisher categoryByPublisher)
        {
            if (ManageSession.HasRole("CREATE_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    db.CategoryByPublishers.Add(categoryByPublisher);
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thêm danh mục NXB - NPH thành công!",
                    };
                    return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
                }
                return View(categoryByPublisher);
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
                CategoryByPublisher categoryByPublisher = db.CategoryByPublishers.Find(id);
                if (categoryByPublisher == null)
                {
                    return HttpNotFound();
                }
                return View(categoryByPublisher);
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
        public ActionResult Edit( CategoryByPublisher categoryByPublisher)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(categoryByPublisher).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa danh mục tác giả thành công!",
                    };
                    return View();
                }
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
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
                CategoryByPublisher categoryByPublisher = db.CategoryByPublishers.Find(id);
                db.CategoryByPublishers.Remove(categoryByPublisher);
                db.SaveChanges();
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa danh mục thành công!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 3 });
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
            CategoryByPublisher categoryByPublisher = db.CategoryByPublishers.Find(id);
            if (categoryByPublisher == null)
            {
                return HttpNotFound();
            }
            return View(categoryByPublisher);
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
