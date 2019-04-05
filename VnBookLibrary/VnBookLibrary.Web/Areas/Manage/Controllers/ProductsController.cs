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
        private UnitOfWork UoW;
        public ProductsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        public ActionResult Index(int? CategoryLv1Id, int? CategoryLv2Id, int? CategoryAuthorId, int? CategoryPublisherId)
        {
            ViewBag.CategoryLv1Id = CategoryLv1Id;
            ViewBag.CategoryLv2Id = CategoryLv2Id;
            ViewBag.CategoryAuthorId = CategoryAuthorId;
            ViewBag.CategoryPublisherId = CategoryPublisherId;
            return View();
        }
        public PartialViewResult _GetProductByPage(string Search, int? CategoryLv1Id, int? CategoryLv2Id, int? CategoryAuthorId, int? CategoryPublisherId, int? TypeDisplay = 1, int pageNumber = 1, int pageSize = 10)
        {
            bool isSearch = true;
            if ((Search == null || Search == "") && !(CategoryLv2Id > 0) && !(CategoryLv1Id > 0) && !(CategoryAuthorId > 0) && !(CategoryPublisherId > 0))
                isSearch = false;
            ViewBag.isSearch = isSearch;
            ViewBag.Search = Search;
            ViewBag.CategoryLv1Id = CategoryLv1Id;
            ViewBag.CategoryLv2Id = CategoryLv2Id;
            ViewBag.TypeDisplay = TypeDisplay;
            ViewBag.CategoryAuthorId = new SelectList(UoW.CategoryByAuthorRepository.GetAll(), "CategoryAuthorId", "CategoryAuthorName", CategoryAuthorId);
            ViewBag.CategoryPublisherId = new SelectList(UoW.CategoryByPublisherRepository.GetAll(), "CategoryByPublisherId", "CategoryByPublisherName", CategoryPublisherId);
            return PartialView(UoW.ProductRepository.GetPageListProduct(Search, CategoryLv1Id, CategoryLv2Id, CategoryAuthorId, CategoryPublisherId, pageNumber, pageSize));
        }

        [HasRole(RoleCode = "CREATE_PRODUCT")]
        public ActionResult Create()
        {
            ViewBag.CategoryByAuthorId = new SelectList(UoW.CategoryByAuthorRepository.GetAll(), "CategoryAuthorId", "CategoryAuthorName");
            ViewBag.CategoryByPublisherId = new SelectList(UoW.CategoryByPublisherRepository.GetAll(), "CategoryByPublisherId", "CategoryByPublisherName");
            return View();
        }

        [HasRole(RoleCode = "CREATE_PRODUCT")]
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UoW.ProductRepository.Insert(product);
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
                ViewBag.CategoryByAuthorId = new SelectList(UoW.CategoryByAuthorRepository.GetAll(), "CategoryAuthorId", "CategoryAuthorName");
                ViewBag.CategoryByPublisherId = new SelectList(UoW.CategoryByPublisherRepository.GetAll(), "CategoryByPublisherId", "CategoryByPublisherName");
                return View(product);
            }
        }
        [HasRole(RoleCode = "EDIT_PRODUCT")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            Product product = UoW.ProductRepository.Find(id);
            if (product == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            ViewBag.CategoryByAuthorId = new SelectList(UoW.CategoryByAuthorRepository.GetAll(), "CategoryAuthorId", "CategoryAuthorName", product.CategoryByAuthorId);
            ViewBag.CategoryByPublisherId = new SelectList(UoW.CategoryByPublisherRepository.GetAll(), "CategoryByPublisherId", "CategoryByPublisherName", product.CategoryByPublisherId);
            return View(product);
        }

        [HasRole(RoleCode = "EDIT_PRODUCT")]
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                UoW.ProductRepository.Update(product);
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
        [HasRole(RoleCode = "DELETE_PRODUCT")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ManageSession.HasRole("DELETE_PRODUCT"))
            {
                if (UoW.ProductRepository.Delete(id) > 0)
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Xóa sách thành công!",
                    };
                    return RedirectToAction("Index", "Products", new { Area = "Manage" });
                }
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            TempData["Notify"] = new JsonResultBO()
            {
                Status = false,
                Message = "Bạn không có quyền này!",
            };
            return RedirectToAction("Index", "Products", new { Area = "Manage" });
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
