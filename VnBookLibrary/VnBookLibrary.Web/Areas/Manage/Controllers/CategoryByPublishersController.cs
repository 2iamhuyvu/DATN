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
                CategoryByPublisher categoryByPublisher = UoW.CategoryByPublisherRepository.Find(id);
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
                UoW.CategoryByPublisherRepository.Delete(id);
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
                UoW.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
