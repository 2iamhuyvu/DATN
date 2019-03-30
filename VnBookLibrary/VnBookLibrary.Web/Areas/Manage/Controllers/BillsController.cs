﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using PagedList;
using PagedList.Mvc;
using VnBookLibrary.Web.Areas.Manage.Customizes;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Repository.Repositories;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class BillsController : Controller
    {
        private VnBookLibraryDbContext db ;
        private UnitOfWork UoW;        
        public BillsController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);            
        }
        public ActionResult Index()
        {            
            return View();
        }
        public PartialViewResult _GetBillByStatus(int? status=-1,int page=1,int pageSize=15)
        {
            List<Bill> bills = UoW.BillRepository.GetAll().ToList();
            if (status > 0)
            {
                bills = bills.Where(x => x.Status == status).ToList();
            }
            ViewBag.StatusBill = status;
            return PartialView(bills.OrderBy(x => x.PayDate).ToPagedList(page,bills.Count==0?100: bills.Count));
        }
        public ActionResult _modalDetail(int id)
        {
            ViewBag.Bill = UoW.BillRepository.Find(id);            
            ViewBag.ListBillDetail = UoW.BillRepository.GetAll().Where(x => x.BillId == id).ToList();
            return PartialView();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = UoW.BillRepository.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }
        public ActionResult ConfirmDeliver(int billId)
        {
            Bill bill = new Bill();
            bill = UoW.BillRepository.Find(billId);
            if (bill == null)
            {
                TempData["Notify"] = new JsonResultBO(false) { Message = "Có lỗi, hãy thử lại" };
            }
            else
            {
                bill.Status = Constants.STATTUS_BILL_DELIVERING;
                UoW.BillRepository.Update(bill);
                TempData["Notify"] = new JsonResultBO(true) { Message = "Đã chuyển đổi trạng thái thành đang giao hàng" };
            }
            return RedirectToAction("Index", "Bills", new { Area = "Manage" });
        }

        public ActionResult ConfirmPaid(int billId)
        {
            Bill bill = new Bill();
            bill = UoW.BillRepository.Find(billId);
            if (bill == null)
            {
                TempData["Notify"] = new JsonResultBO(false) { Message = "Có lỗi, hãy thử lại" };
            }
            else
            {
                bill.Status = Constants.STATTUS_BILL_PAID;
                UoW.BillRepository.Update(bill);
                TempData["Notify"] = new JsonResultBO(true) { Message = "Đã chuyển đổi trạng thái đã thanh toán" };
            }
            return RedirectToAction("Index", "Bills", new { Area = "Manage" });
        }
        public ActionResult ConfirmReturned(int billId)
        {
            Bill bill = new Bill();
            bill = UoW.BillRepository.Find(billId);
            if (bill == null)
            {
                TempData["Notify"] = new JsonResultBO(false) { Message = "Có lỗi, hãy thử lại" };
            }
            else
            {
                bill.Status = Constants.STATTUS_BILL_RETURNED;
                UoW.BillRepository.Update(bill);
                TempData["Notify"] = new JsonResultBO(false) { Message = "Đã chuyển đổi trạng thái bị trả về" };
            }
            return RedirectToAction("Index", "Bills", new { Area = "Manage" });
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