using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class BookCategoriesController : Controller
    {
        private VnBookLibraryDbContext db;
        private CategoryLv1Repository categoryLv1Repository;
        private CategoryLv2Repository categoryLv2Repository;
        private CategoryByAuthorRepository categoryByAuthorRepository;
        private CategoryByPublisherRepository categoryByPublisherRepository;
        public BookCategoriesController()
        {
            db = new VnBookLibraryDbContext();
            categoryByPublisherRepository = new CategoryByPublisherRepository(db);
            categoryLv1Repository = new CategoryLv1Repository(db);
            categoryLv2Repository = new CategoryLv2Repository(db);
            categoryByAuthorRepository = new CategoryByAuthorRepository(db);
        }
        // GET: Manage/BookCategories
        public ActionResult Index(int? displayCategory)
        {
            //displayCategory=1 --> hien danh mục các cấp
            //displayCategory=2 --> hien danh mục tác giả
            //displayCategory=3 --> hien danh mục nxb nph
            ViewBag.displayCategory = displayCategory ?? 0;
            ViewBag.ListCategoryLv1 = categoryLv1Repository.GetAll().OrderBy(x => x.OrderDisplay).ToList();
            ViewBag.ListCategoryLv2 = categoryLv2Repository.GetAll().OrderBy(x => x.CategoryLv1Id).ThenBy(x => x.CategoryLv2Id).ToList();
            ViewBag.ListCategoryByAuthor = categoryByAuthorRepository.GetAll().OrderBy(x => x.CategoryAuthorName).ToList();
            ViewBag.ListCategoryByPublisher = categoryByPublisherRepository.GetAll().OrderBy(x => x.CategoryByPublisherName).ToList();
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult _DropdownCategory(int? categoryLv1, int? categoryLv2,bool? required=false)
        {

            ViewBag.required = required;
            if (categoryLv1 > 0)
            {
                ViewBag.CategoryLv1Id = new SelectList(db.CategoryLv1s, "CategoryLv1Id", "CategoryLv1Name", categoryLv1);
                ViewBag.CategoryLv2Id = new SelectList(db.CategoryLv2s.Where(x => x.CategoryLv1Id == categoryLv1).ToList(), "CategoryLv2Id", "CategoryLv2Name",categoryLv2);
            }
            else
            {
                ViewBag.CategoryLv1Id = new SelectList(db.CategoryLv1s, "CategoryLv1Id", "CategoryLv1Name");
                if (categoryLv2 > 0)
                {
                    ViewBag.CategoryLv2Id = new SelectList(db.CategoryLv2s.Where(x => x.CategoryLv2Id == categoryLv2).ToList(), "CategoryLv2Id", "CategoryLv2Name", categoryLv2);
                }
                else
                {
                    ViewBag.CategoryLv2Id = null;
                }
            }            
            return PartialView();
        }
    }
}
