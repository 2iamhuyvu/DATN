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
    public class CategoryLv2Controller : Controller
    {
        private VnBookLibraryDbContext db = new VnBookLibraryDbContext();                      
        public ActionResult Create()
        {                        
            if (ManageSession.HasRole("CREATE_CATEGORY"))
            {
                ViewBag.CategoryLv1Id = new SelectList(db.CategoryLv1s, "CategoryLv1Id", "CategoryLv1Name");
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
        public ActionResult Create(CategoryLv2 categoryLv2)
        {
            if (ManageSession.HasRole("CREATE_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    db.CategoryLv2s.Add(categoryLv2);
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thêm danh mục cấp 2 thành công!",
                    };
                    return RedirectToAction("Index", "BookCategories", new { Area = "Manage", displayCategory = 1 });
                }
                ViewBag.CategoryLv1Id = new SelectList(db.CategoryLv1s, "CategoryLv1Id", "CategoryLv1Name");
                return View(categoryLv2);
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

        // GET: Manage/CategoryLv2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CategoryLv2 categoryLv2 = db.CategoryLv2s.Find(id);
                if (categoryLv2 == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryLv1Id = new SelectList(db.CategoryLv1s.ToList(), "CategoryLv1Id", "CategoryLv1Name", categoryLv2.CategoryLv1Id);
                return View(categoryLv2);
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
        public ActionResult Edit(CategoryLv2 categoryLv2)
        {
            if (ManageSession.HasRole("EDIT_CATEGORY"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(categoryLv2).State = EntityState.Modified;                    
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa danh mục cấp 2 thành công!",
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
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_CATEGORY"))
            {
                CategoryLv2 categoryLv2 = db.CategoryLv2s.Find(id);
                db.CategoryLv2s.Remove(categoryLv2);
                db.SaveChanges();                
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
