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
using VnBookLibrary.Web.Areas.Manage.Customizes;
using PagedList.Mvc;
using PagedList;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Repository.Commons;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]      
    public class ProductsController : Controller
    {
        private VnBookLibraryDbContext db;
        private ProductRepository productRepository;
        public ProductsController()
        {
            db = new VnBookLibraryDbContext();
            productRepository = new ProductRepository(db);
        }          
        public ActionResult Index(int? CategoryLv1Id, int? CategoryLv2Id, int? CategoryAuthorId, int? CategoryPublisherId)
        {
            ViewBag.CategoryLv1Id = CategoryLv1Id;
            ViewBag.CategoryLv2Id = CategoryLv2Id;
            ViewBag.CategoryAuthorId = CategoryAuthorId;            
            ViewBag.CategoryPublisherId = CategoryPublisherId;
            return View();
        }        
        public PartialViewResult _GetProductByPage(string Search,int? CategoryLv1Id, int? CategoryLv2Id,int? CategoryAuthorId,int? CategoryPublisherId,int? TypeDisplay=1, int pageNumber = 1, int pageSize = 10)
        {
            bool isSearch = true;
            if ((Search == null || Search == "") && !(CategoryLv2Id > 0) && !(CategoryLv1Id > 0) && !(CategoryAuthorId > 0) && !(CategoryPublisherId > 0))
                isSearch = false;
            ViewBag.isSearch = isSearch;
            ViewBag.Search = Search;
            ViewBag.CategoryLv1Id = CategoryLv1Id;
            ViewBag.CategoryLv2Id = CategoryLv2Id;
            ViewBag.TypeDisplay = TypeDisplay;
            ViewBag.CategoryAuthorId = new SelectList(db.CategoryByAuthors, "CategoryAuthorId", "CategoryAuthorName",CategoryAuthorId);
            ViewBag.CategoryPublisherId = new SelectList(db.CategoryByPublishers, "CategoryByPublisherId", "CategoryByPublisherName",CategoryPublisherId);
            return PartialView(productRepository.GetPageListProduct(Search,CategoryLv1Id,CategoryLv2Id,CategoryAuthorId,CategoryPublisherId, pageNumber, pageSize));
        }           
        public ActionResult Create()
        {
            if (ManageSession.HasRole("CREATE_PRODUCT"))
            {
                ViewBag.CategoryByAuthorId = new SelectList(db.CategoryByAuthors, "CategoryAuthorId", "CategoryAuthorName");
                ViewBag.CategoryByPublisherId = new SelectList(db.CategoryByPublishers, "CategoryByPublisherId", "CategoryByPublisherName");                
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm sách!",
                };
                return RedirectToAction("Index", "BookCategories", new { Area = "Manage" });
            }
        }
        [HttpPost]        
        public ActionResult Create(Product product)
        {
            if (ManageSession.HasRole("CREATE_PRODUCT"))
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thêm sách thành công!",
                    };
                    return Json(new JsonResultBO(true));
                }
                catch
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Lỗi!",
                    };
                    return Json(new JsonResultBO(false));
                }
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm sách!",
                };
                return Json(new JsonResultBO(false));
            }
        }
        public ActionResult Edit(int? id)
        {
            if (ManageSession.HasRole("EDIT_PRODUCT"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryByAuthorId = new SelectList(db.CategoryByAuthors, "CategoryAuthorId", "CategoryAuthorName", product.CategoryByAuthorId);
                ViewBag.CategoryByPublisherId = new SelectList(db.CategoryByPublishers, "CategoryByPublisherId", "CategoryByPublisherName", product.CategoryByPublisherId);
                return View(product);
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền sửa sách!",
                };
                return RedirectToAction("Index", "Products", new { Area = "Manage" });
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ManageSession.HasRole("EDIT_PRODUCT"))
            {
                try
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa sách thành công!",
                    };
                    return Json(new JsonResultBO(true));
                }
                catch
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Lỗi!",
                    };
                    return Json(new JsonResultBO(false));
                }
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền sửa sách!",
                };
                return Json(new JsonResultBO(false));
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_PRODUCT"))
            {

                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Xóa sách thành công!",
                };
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền xóa sách!",
                };
                return RedirectToAction("Index", "Products", new { Area = "Manage" });
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
