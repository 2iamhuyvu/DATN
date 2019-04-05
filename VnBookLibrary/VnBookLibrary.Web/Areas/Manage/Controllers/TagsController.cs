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

        // GET: Manage/Tags
        public ActionResult Index()
        {            
            return View(db.Tags.ToList());
        }        
        [HasRole(RoleCode ="CREATE_TAG")]
        // GET: Manage/Tags/Create
        public ActionResult Create()
        {
            ViewBag.Products = UoW.ProductRepository.GetAll();
            return View();
        }
        [HasRole(RoleCode = "CREATE_TAG")]
        [HttpPost]        
        public ActionResult Create(Tag tag)
        {
            List<string> ListProductId = new List<string>();
            if (Request.Form.GetValues("ListProductId") != null)
            {
                ListProductId = Request.Form.GetValues("ListProductId").ToList();
            }
            if (ModelState.IsValid)
            {
                if (tag.TagId.Trim() == "")
                {
                    ModelState.AddModelError("TagId", "Mã Tag không được để toàn là ký tự trống!");                    
                }
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
                if(UoW.TagRepository.Insert(tag)>0&&ListProductId !=null&& ListProductId.Count > 0)
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
        public ActionResult Edit(string id)
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
        public ActionResult _Edit(string id)
        {
            ViewBag.Tag_Products = db.Tag_Products.Where(x => x.TagId == id).ToList();
            ViewBag.Products = db.Products.ToList();
            Tag tag = db.Tags.Find(id);
            return PartialView(tag);
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        [HttpPost]        
        public ActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        [HttpPost]
        public ActionResult DeleteTag_Product(string TagId,int ProductId)
        {
            if(ManageSession.HasRole("EDIT_TAG"))
            {
                Tag_Product tag_Product = db.Tag_Products.FirstOrDefault(x => x.TagId == TagId && x.ProductId == ProductId);
                if (tag_Product != null)
                {
                    UoW.Tag_ProductRepository.Delete(tag_Product);
                    return Json(new JsonResultBO(true)
                    {
                        Message = "Xóa khỏi nhãn thành công"
                    });
                }
                return Json(new JsonResultBO(false)
                {
                    Message = "Không tồn tại sản phẩm này trong nhãn"
                });
            }
            return Json(new JsonResultBO(false)
            {
                Message = "Bạn không có quyền này"
            });
        }
        [HasRole(RoleCode = "EDIT_TAG")]
        [HttpPost]
        public ActionResult MultiDeleteTag_Product(string TagId, List<int> Tag_ProductId)
        {
            if (ManageSession.HasRole("EDIT_TAG"))
            {
                if(Tag_ProductId==null|| Tag_ProductId.Count==0)
                {
                    return Json(new JsonResultBO(false)
                    {
                        Message = "Không có sách nào được chọn!"
                    });
                }
                foreach (var item in Tag_ProductId)
                {
                    Tag_Product tag_Product = db.Tag_Products.FirstOrDefault(x => x.TagId.Equals(TagId)&& x.ProductId == item);
                    if (tag_Product != null)
                    {
                        db.Tag_Products.Remove(tag_Product);
                    }
                }
                db.SaveChanges();
                return Json(new JsonResultBO(true)
                {
                    Message = "Xóa khỏi nhãn thành công"
                });
            }
            return Json(new JsonResultBO(false)
            {
                Message = "Bạn không có quyền này"
            });
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
