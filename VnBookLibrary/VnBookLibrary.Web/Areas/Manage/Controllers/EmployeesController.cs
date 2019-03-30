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
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Areas.Manage.Customizes;
using VnBookLibrary.Web.Areas.Manage.Models;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    public class EmployeesController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public EmployeesController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }        
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                ManageSessionModel manageSessionModel = (ManageSessionModel)ManageSession.GetSession(Constants.MANAGE_SESSION);
                if (!PasswordEncryption.GetVnBookLibraryCode(model.CurrentPassword).Equals(manageSessionModel.SessionEmployee.Password))
                {
                    ModelState.AddModelError("CurrentPassword", "Mật khẩu không đúng!");
                    return View(model);
                }
                else
                {
                    Employee e = UoW.EmployeeRepository.Find(manageSessionModel.SessionEmployee.EmployeeId);
                    e.Password = PasswordEncryption.GetVnBookLibraryCode(model.NewPassword);
                    e.RePassword = PasswordEncryption.GetVnBookLibraryCode(model.NewPassword);
                    UoW.EmployeeRepository.Update(e);
                    ManageSession.RemoveSession(Constants.MANAGE_SESSION);
                    TempData["NotifyLogin"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thay đôi mật khẩu thành công, mời đăng nhập lại!"
                    };
                    return RedirectToAction("Index", "Login");
                }
            }
            return View(model);
        }

        // GET: Manage/Employees
        public ActionResult Index()
        {
            var employees = UoW.EmployeeRepository.GetAll();
            return View(employees.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.EmployeeTypeId = new SelectList(UoW.EmployeeTypeRepository.GetAll(), "EmployeeTypeId", "EmployeeTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (db.Employees.FirstOrDefault(x => x.LoginName.Equals(employee.LoginName)) != null)
                {
                    ModelState.AddModelError("LoginName", "Tên đăng nhập này đã tồn tại");
                    return View(employee);
                }
                if (employee.Password.Length<5||employee.LoginName.Trim() == "" || employee.Password.Trim() == null || employee.EmployeeName.Trim() == "")
                {
                    if(employee.LoginName.Trim() == "")
                        ModelState.AddModelError("LoginName","Không được để trống tên đăng nhập!");
                    if (employee.Password.Trim() == "")
                        ModelState.AddModelError("Password", "Không được để trống mật khẩu!");
                    if (employee.Password.Length < 5)
                        ModelState.AddModelError("Password", "Mật khẩu tối thiểu 5 ký tự");
                    if (employee.EmployeeName.Trim() == "")
                        ModelState.AddModelError("EmployeeName", "Không được để trống tên nhân viên!");
                    return View(employee);                    
                }
                employee.Password = PasswordEncryption.GetVnBookLibraryCode(employee.Password);
                UoW.EmployeeRepository.Insert(employee);
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeTypeId = new SelectList(UoW.EmployeeTypeRepository.GetAll(), "EmployeeTypeId", "EmployeeTypeName", employee.EmployeeTypeId);
            return View(employee);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = UoW.EmployeeRepository.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeTypeId = new SelectList(UoW.EmployeeTypeRepository.GetAll(), "EmployeeTypeId", "EmployeeTypeName", employee.EmployeeTypeId);
            return View(employee);
        }

        // POST: Manage/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (db.Employees.FirstOrDefault(x => x.LoginName.Equals(employee.LoginName) && x.EmployeeId != employee.EmployeeId) != null)
                {
                    ModelState.AddModelError("LoginName", "Tên đăng nhập này đã tồn tại");
                    return View(employee);
                }
                else
                {
                    UoW.EmployeeRepository.Update(employee);
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa thông tin thành công!",
                    };
                    return RedirectToAction("Index");
                }
            }
            ViewBag.EmployeeTypeId = new SelectList(UoW.EmployeeTypeRepository.GetAll(), "EmployeeTypeId", "EmployeeTypeName", employee.EmployeeTypeId);
            return View(employee);
        }

        // GET: Manage/Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = UoW.EmployeeRepository.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Manage/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UoW.EmployeeRepository.Delete(id);
            return RedirectToAction("Index");
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