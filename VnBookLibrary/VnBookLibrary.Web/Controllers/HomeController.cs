using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Models;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using VnBookLibrary.Web.Areas.Manage.Customizes;
using System.Threading.Tasks;

namespace VnBookLibrary.Web.Controllers
{
    public class HomeController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public HomeController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }


        public ActionResult Index()
        {
            if (Session[Constants.CUSTOMER_SESSION] != null)
            {
                var customer = (Customer)Session[Constants.CUSTOMER_SESSION];
                ViewBag.ReCommendProductByCustomer = UoW.RecommendRepository.GetRecommendProductByCustomer(customer.CustomerId);
            }
            ViewBag.AllTag = UoW.TagRepository.GetAll().OrderBy(x=>x.OrderDisplay).ToList();
            ViewBag.HomeTag = UoW.TagRepository.GetAll().OrderBy(x => x.OrderDisplay).ToList().Where(x => x.IsDisplay).ToList();            
            return View();
        }

        public ActionResult _RecommenproductByCart()
        {
            List<Product> listProductCart = new List<Product>();
            if (Session[Constants.CART_SESSION] != null)
            {
                listProductCart = ((List<CartVM>)Session[Constants.CART_SESSION]).Select(x => x.Product).ToList();
            }
            List<Product>  model = UoW.RecommendRepository.GetRecommendProductByListProduct(listProductCart);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CommentProduct(string Comments, int ProductId, int CustomerId)
        {
            UoW.CommentProductRepository.Insert(new CommentProduct()
            {
                AlowDisplay = false,
                CommentDate = DateTime.Now,
                Comments = Comments,
                CustomerId = CustomerId,
                ProductId = ProductId,
            });
            return Json(new JsonResultBO(true));
        }
        [HttpPost]
        public ActionResult RateProduct(int Point, int ProductId, int CustomerId)
        {
            UoW.RateProductRepository.Delete(ProductId, CustomerId);
            if (Point > 0 && Point < 6)
            {
                UoW.RateProductRepository.Insert(new RateProduct()
                {
                    CustomerId = CustomerId,
                    ProductId = ProductId,
                    Point = Point,
                });
            }
            return Json(new JsonResultBO(true));
        }

        [HttpPost]
        public ActionResult LikeProduct(int StatusLike, int ProductId, int CustomerId)
        {
            if (StatusLike == 1)
            {
                UoW.LikeProductRepository.Delete(ProductId, CustomerId);
            }
            else
            {
                UoW.LikeProductRepository.Insert(new LikeProduct()
                {
                    CustomerId = CustomerId,
                    ProductId = ProductId
                });
            }
            return Json(new JsonResultBO(true));
        }

        public ActionResult Books(string Search, int? CategoryLv1Id, int? CategoryLv2Id, int? CategoryAuthorId, int? CategoryPublisherId)
        {
            var cate1 = UoW.CategoryLv1Repository.Find(CategoryLv1Id ?? 0);
            var cate2 = UoW.CategoryLv2Repository.Find(CategoryLv2Id ?? 0);
            var cateAuthor = UoW.CategoryByAuthorRepository.Find(CategoryAuthorId ?? 0);
            var catePublisher = UoW.CategoryByPublisherRepository.Find(CategoryPublisherId ?? 0);
            if (cate1 != null)
            {
                ViewBag.Title = "Sách " + cate1.CategoryLv1Name + " | VnBook";
            }
            else if (cate2 != null)
            {
                ViewBag.Title = "Sách " + cate2.CategoryLv2Name + " | VnBook";
            }
            else if (cateAuthor != null)
            {
                ViewBag.Title = "Sách theo Tác giả " + cateAuthor.CategoryAuthorName + " | VnBook";
            }
            else if (catePublisher != null)
            {
                ViewBag.Title = "Sách theo NXB - NPH " + catePublisher.CategoryByPublisherName + " | VnBook";
            }
            else if (Search != null && Search != "")
            {
                ViewBag.Title = "Kết quả tìm kiếm:" + Search + " | VnBook";
            }
            else
            {
                ViewBag.Title = "Trang chủ | VnBook";
            }
            bool isSearch = true;
            if (Search == null || Search == "")
                isSearch = false;
            ViewBag.isSearch = isSearch;
            ViewBag.Search = Search;
            ViewBag.CategoryLv1Id = cate1 == null ? null : CategoryLv1Id;
            ViewBag.CategoryLv2Id = cate2 == null ? null : CategoryLv2Id;
            ViewBag.CategoryAuthorId = cateAuthor == null ? null : CategoryAuthorId;
            ViewBag.CategoryPublisherId = catePublisher == null ? null : CategoryPublisherId;
            return View();
        }

        [Route("tin-tuc")]
        public ActionResult News()
        {
            return View(UoW.NewsRepository.GetAll());
        }
        [Route("tin-tuc/{id:min(1)}")]
        public ActionResult DetailsNews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = UoW.NewsRepository.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
        public ActionResult _GetProductByPage(string Search, int? CategoryLv1Id, int? CategoryLv2Id, int? CategoryAuthorId, int? CategoryPublisherId, int page = 1, int pageSize = 10)
        {
            string href = "";
            var cate1 = UoW.CategoryLv1Repository.Find(CategoryLv1Id ?? 0);
            var cate2 = UoW.CategoryLv2Repository.Find(CategoryLv2Id ?? 0);
            var cateAuthor = UoW.CategoryByAuthorRepository.Find(CategoryAuthorId ?? 0);
            var catePublisher = UoW.CategoryByPublisherRepository.Find(CategoryPublisherId ?? 0);
            if (cate1 != null)
            {
                href = "";
                href += "<a href='/'>Trang chủ</a><i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i>" + "<a href='/Home/Books?CategoryLv1Id=" + cate1.CategoryLv1Id + "'>" + cate1.CategoryLv1Name + "</a>";
            }
            if (cate2 != null)
            {
                href = "";
                cate1 = cate2.CategoryLv1;
                href += "<a href='/'>Trang chủ</a><i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i>"
                    + "<a href='/Home/Books?CategoryLv1Id="
                    + cate1.CategoryLv1Id + "'>"
                    + cate1.CategoryLv1Name + "</a><i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i>"
                    + "<a href='/Home/Books?CategoryLv2Id="
                    + cate2.CategoryLv2Id + "'>"
                    + cate2.CategoryLv2Name + "</a>";
            }
            if (cateAuthor != null)
            {
                href = "";
                href += "<a href='/'>Trang chủ</a><i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i>"
                    + "<a href='/Home/Books?CategoryAuthorId="
                    + cateAuthor.CategoryAuthorId + "'>"
                    + cateAuthor.CategoryAuthorName + "</a>";
            }
            if (catePublisher != null)
            {
                href = "";
                href += "<a href='/'>Trang chủ</a><i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i>"
                    + "<a href='/Home/Books?CategoryAuthorId="
                    + catePublisher.CategoryByPublisherId + "'>"
                    + catePublisher.CategoryByPublisherName + "</a>";
            }
            if (href != "")
                href += "<i class='fa fa-lg' style='font-weight:normal'>&nbsp;&#xf0da;&nbsp;</i><a href='javascript:void(0)'>Trang " + page + "</a>";
            ViewBag.Href = href;
            ViewBag.Search = Search;
            ViewBag.CategoryLv1Id = CategoryLv1Id;
            ViewBag.CategoryLv2Id = CategoryLv2Id;
            ViewBag.CategoryAuthorId = CategoryAuthorId;
            ViewBag.CategoryPublisherId = CategoryPublisherId;
            var model = UoW.ProductRepository.GetPageListProduct(Search, CategoryLv1Id, CategoryLv2Id, CategoryAuthorId, CategoryPublisherId, page, pageSize);
            return PartialView(model);
        }
        public ActionResult DetailProduct(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/BadRequest.cshtml");
            }
            Product product = new Product();
            product = UoW.ProductRepository.Find(id);
            if (product == null)
            {
                return View("~/Views/Shared/BadRequest.cshtml");
            }
            ViewBag.Title = product.ProductName + " | VnBook";
            ViewBag.ProductId = id;
            ViewBag.Product = product;
            ViewBag.RecommendProductByProduct = UoW.RecommendRepository.GetRecommendProductByProduct(product.ProductId);
            if (Session[Constants.CUSTOMER_SESSION] != null)
            {
                var customer = (Customer)Session[Constants.CUSTOMER_SESSION];
                ViewBag.ReCommendProductByCustomer = UoW.RecommendRepository.GetRecommendProductByCustomer(customer.CustomerId);
            }
            return View();
        }
        public ActionResult _DetailProduct(int productId)
        {
            Customer customer = null;
            RateProduct rateProduct = null;
            bool liked = false;
            List<CommentProduct> commentProducts = null;
            List<RateProduct> rateProducts = new List<RateProduct>();
            rateProducts = UoW.RateProductRepository.GetAll().Where(x => x.ProductId == productId).ToList();
            commentProducts = UoW.CommentProductRepository.GetAll().Where(x => x.ProductId == productId && x.AlowDisplay == true).ToList();
            if (Session[Constants.CUSTOMER_SESSION] != null)
            {
                customer = (Customer)Session[Constants.CUSTOMER_SESSION];
                rateProduct = UoW.RateProductRepository.GetAll().FirstOrDefault(x => x.ProductId == productId && x.CustomerId == customer.CustomerId);
                liked = UoW.LikeProductRepository.GetAll().FirstOrDefault(x => x.ProductId == productId && x.CustomerId == customer.CustomerId) == null ? false : true;
            }
            ViewBag.RecommendProductByProduct = UoW.RecommendRepository.GetRecommendProductByProduct(productId);
            ViewBag.CommentProducts = commentProducts;
            ViewBag.RateProducts = rateProducts;
            ViewBag.NumberLike = UoW.LikeProductRepository.GetAll().Where(x => x.ProductId == productId).ToList().Count;
            ViewBag.Liked = liked;
            ViewBag.Rate = rateProduct;
            ViewBag.Customer = customer;
            Product product = new Product();
            product = UoW.ProductRepository.Find(productId);
            ViewBag.Product = product;
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            int numberOfBook = 0;
            Product product = new Product();
            product = UoW.ProductRepository.Find(productId);
            if (product != null)
            {
                List<CartVM> listCart = new List<CartVM>();
                if (Session[Constants.CART_SESSION] != null)
                {
                    listCart = (List<CartVM>)Session[Constants.CART_SESSION];
                }
                bool kt = false;
                foreach (var item in listCart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        kt = true;
                        item.Quantity += 1;
                    }
                    numberOfBook += item.Quantity;
                }
                if (!kt)
                {
                    numberOfBook += 1;
                    listCart.Add(new CartVM() { Product = product, Quantity = 1 });
                }
                Session[Constants.CART_SESSION] = listCart;
                return Json(new JsonResultBO(true) { Message = "Đã thêm vào giỏ!", Param = numberOfBook });
            }
            else
            {
                return Json(new JsonResultBO(false) { Message = "Không tồn tại sách này!" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmBuyProduct(Bill bill)
        {
            Customer customer = (Customer)Session[Constants.CUSTOMER_SESSION];
            if (customer != null)
                bill.CustomerId = customer.CustomerId;
            bill.BuyDate = DateTime.Now;
            bill.Status = Constants.STATTUS_BILL_NEW;
            List<CartVM> listCart = new List<CartVM>();
            if (Session[Constants.CART_SESSION] != null)
            {
                listCart = (List<CartVM>)Session[Constants.CART_SESSION];
            }
            decimal intoMoney = 0;
            if (listCart.Count > 0)
            {
                foreach (var cartVM in listCart)
                {
                    intoMoney += cartVM.Quantity * cartVM.Product.Price;
                }
                bill.IntoMoney = intoMoney;
                bill.TotalNotSale = intoMoney;
                UoW.BillRepository.Insert(bill);
                List<BillDetail> billDetails = new List<BillDetail>();
                foreach (var cartVM in listCart)
                {
                    BillDetail billDetail = new BillDetail()
                    {
                        ProductId = cartVM.Product.ProductId,
                        Quantity = cartVM.Quantity,
                        BillId = bill.BillId,
                    };
                    billDetails.Add(billDetail);
                }
                await UoW.RecommendRepository.InsertOrUpdateAsync(billDetails);
                UoW.BillDetailRepository.InsertRange(billDetails);
                Session[Constants.CART_SESSION] = null;
                return Json(new JsonResultBO(true) { Message = "Cám ơn bạn đã mua sách, Sách sẽ sớm được gửi cho bạn!" });
            }
            else
            {
                return Json(new JsonResultBO(false) { Message = "Có lỗi,Xin hãy thử lại!" });
            }
        }

        [Route("gio-hang")]
        public ActionResult ViewCart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult _PartialViewCart()
        {
            List<CartVM> listCart = new List<CartVM>();
            if (Session[Constants.CART_SESSION] != null)
            {
                listCart = (List<CartVM>)Session[Constants.CART_SESSION];
            }
            ViewBag.ListCart = listCart;
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChangeQuantity(int productId, int quantity)
        {
            int numberOfBook = 0;
            List<CartVM> listCart = new List<CartVM>();
            if (Session[Constants.CART_SESSION] != null)
            {
                listCart = (List<CartVM>)Session[Constants.CART_SESSION];
            }
            foreach (var item in listCart)
            {
                if (item.Product.ProductId == productId)
                {
                    item.Quantity = quantity;
                }
                numberOfBook += item.Quantity;
            }
            Session[Constants.CART_SESSION] = listCart;
            return Json(new JsonResultBO(true)
            {
                Message = "Thay đổi thành công!",
                Param = numberOfBook,
            });
        }
        [HttpPost]
        public ActionResult GetProvince()
        {
            return Json(UoW.ProvinceRepository.GetAll().Select(x => new { x.ProvinceName, x.ProvinceId }).OrderBy(x => x.ProvinceName).ToList());
        }
        [HttpPost]
        public ActionResult GetDistrictByProvince(int provinceId)
        {
            return Json(UoW.DistrictRepository.GetAll().Where(x => x.ProvinceId == provinceId).Select(x => new { x.DistrictName, x.DistrictId }).ToList());
        }
        [HttpPost]
        public ActionResult GetWardByDistrict(int districtId)
        {
            return Json(UoW.WardRepository.GetAll().Where(x => x.DidtrictId == districtId).Select(x => new { x.WardName, x.WardId }).ToList());
        }
        public ActionResult _ViewCart()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult DeleteFromCart(int productId)
        {
            int s = 0;
            List<CartVM> listCart = new List<CartVM>();
            if (Session[Constants.CART_SESSION] != null)
            {
                listCart = (List<CartVM>)Session[Constants.CART_SESSION];
            }
            CartVM cartVM = new CartVM();
            foreach (var item in listCart)
            {
                if (item.Product.ProductId == productId)
                {
                    cartVM = item;
                }
                else
                {
                    s += Convert.ToInt32(item.Quantity * item.Product.Price);
                }
            }
            listCart.Remove(cartVM);
            Session[Constants.CART_SESSION] = listCart;
            return Json(new JsonResultBO(true) { Message = "Xóa khỏi giỏ hàng thành công!", Param = s.ToString("c", new CultureInfo("vi-VN")) });
        }
        [ChildActionOnly]
        public ActionResult _PartialMenu()
        {
            ViewBag.ListCategoryLv1 = UoW.CategoryLv1Repository.GetAll();
            ViewBag.ListCategoryAuthor = UoW.CategoryByAuthorRepository.GetAll();
            ViewBag.ListCategoryPublisher = UoW.CategoryByPublisherRepository.GetAll();
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(string loginname, string password, string returnUrl)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.LoginName.Equals(loginname) && x.Password.Equals(password));
            if (customer != null)
            {
                if (customer.IsBlock == true)
                {
                    TempData["Notify"] = new JsonResultBO(false) { Message = "Tài khoản bị khóa!" };
                }
                else
                {
                    Session[Constants.CUSTOMER_SESSION] = customer;
                    TempData["Notify"] = new JsonResultBO(true) { Message = "Đăng nhập thành công!" };
                }
            }
            else
            {
                TempData["Notify"] = new JsonResultBO(false) { Message = "Tên đăng nhập hoặc mật khẩu không đúng!" };
            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (db.Customers.FirstOrDefault(x => x.LoginName.Equals(customer.LoginName)) != null)
                {
                    ModelState.AddModelError("LoginName", "Tên đăng nhập này đã tồn tại");
                    return View(customer);
                }
                else
                {
                    customer.IsBlock = false;
                    UoW.CustomerRepository.Insert(customer);
                    Session[Constants.CUSTOMER_SESSION] = customer;
                    TempData["Notify"] = new JsonResultBO(true) { Message = "Đăng ký thành công!" };
                    return RedirectToAction("Index");
                }
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult LogOut()
        {
            Session[Constants.CUSTOMER_SESSION] = null;
            Session[Constants.CART_SESSION] = null;
            return RedirectToAction("Index");
        }
    }
}