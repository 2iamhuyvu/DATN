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

        // GET: Manage/Tags/Create
        public ActionResult Create()
        {
            ViewBag.Products = UoW.ProductRepository.GetAll();
            return View();
        }
        
        [HttpPost]        
        public ActionResult Create(Tag tag)
        {
            List<string> ListProductId = Request.Form.GetValues("ListProductId").ToList();
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

        // GET: Manage/Tags/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }
       
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
