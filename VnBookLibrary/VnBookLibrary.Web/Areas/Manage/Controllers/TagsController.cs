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
    public class TagsController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public TagsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode ="VIEW_TAG")]
        // GET: Manage/Tags
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }
        [HasRole(RoleCode = "CREATE_TAG")]
        // GET: Manage/Tags/Create
        public ActionResult Create()
        {
            ViewBag.Products = UoW.ProductRepository.GetAll();
            return View();
        }
        [HasRole(RoleCode = "CREATE_TAG")]
        [HttpPost]
        public ActionResult Create(Tag tag, List<string> ListProductId)
        {
            if (ModelState.IsValid)
            {
                if (tag.TagName.Trim() == "")
                {
                    ModelState.AddModelError("TagName", "Tên Tag không được để toàn là ký tự trống!");
                }
                if (db.Tags.FirstOrDefault(x => x.TagId == tag.TagId) != null)
                {
                    ModelState.AddModelError("TagId", "Mã Tag đã tồn tại!");
                }
                if (db.Tags.FirstOrDefault(x => x.TagName == tag.TagName) != null)
                {
                    ModelState.AddModelError("TagTagName", "Tên Tag đã tồn tại!");
                }
                if (UoW.TagRepository.Insert(tag) > 0 && ListProductId != null && ListProductId.Count > 0)
                {
                    foreach (var item in ListProductId)
                    {
                        Tag_Product tag_Product = new Tag_Product()
                        {
                            ProductId = Convert.ToInt32(item),
                            TagId = tag.TagId,
                        };
                        db.Tag_Products.Add(tag_Product);
                    }
                    db.SaveChanges();
                }
                TempData["Notify"] = new JsonResultBO(true)
                {
                    Message = "Thêm tag thành công",
                };
                return RedirectToAction("Index");
            }
            ViewBag.Products = UoW.ProductRepository.GetAll();
            return View(tag);
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        // GET: Manage/Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Views/Shared/_BadRequest.cshtml");
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return View("~/Areas/Views/Shared/_BadRequest.cshtml");
            }
            ViewBag.Tag_Products = db.Tag_Products.Where(x => x.TagId == id).ToList();
            ViewBag.Products = db.Products.ToList();
            return View(tag);
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        [HttpPost]
        public ActionResult Edit(Tag tag, List<string> ListProductId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                var a = db.Tag_Products.Where(x => x.TagId.Equals(tag.TagId)).ToList();
                db.Tag_Products.RemoveRange(a);
                db.SaveChanges();
                if (ListProductId != null)
                {
                    foreach (var item in ListProductId)
                    {
                        Tag_Product tag_Product = new Tag_Product()
                        {
                            ProductId = Convert.ToInt32(item),
                            TagId = tag.TagId,
                        };
                        db.Tag_Products.Add(tag_Product);
                    }
                    db.SaveChanges();
                }               
                TempData["Notify"] = new JsonResultBO(true)
                {
                    Message = "Sửa tag thành công",
                };
                return RedirectToAction("Edit", "Tags", new { id = tag.TagId });
            }
            ViewBag.Tag_Products = db.Tag_Products.Where(x => x.TagId == tag.TagId).ToList();
            ViewBag.Products = db.Products.ToList();
            return View(tag);
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        [HttpPost]
        public ActionResult Delete(int TagId)
        {
            if (ManageSession.HasRole("EDIT_TAG"))
            {
                var tag_Product = db.Tag_Products.Where(x => x.TagId == TagId).ToList();
                db.Tag_Products.RemoveRange(tag_Product);
                db.SaveChanges();
                UoW.TagRepository.Delete(TagId);
                TempData["Notify"] = new JsonResultBO(true)
                {
                    Message = "Xóa khỏi nhãn thành công"
                };
                return RedirectToAction("Index", "Tags", new { Area = "Manage" });
            }
            TempData["Notify"] = new JsonResultBO(false)
            {
                Message = "Bạn không có quyền này"
            };
            return RedirectToAction("Index", "Tags", new { Area = "Manage" });
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
