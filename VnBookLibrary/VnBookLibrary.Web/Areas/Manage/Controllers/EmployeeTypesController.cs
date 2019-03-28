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

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{
    [AuthorizeManage]
    [RouteArea("quan-tri")]
    [RoutePrefix("loai-nhan-vien")]
    [Route("{action}")]
    public class EmployeeTypesController : Controller
    {
        private VnBookLibraryDbContext db;
        private EmployeeTypeRepository _employeeTypeRepository;
        public EmployeeTypesController()
        {
            db = new VnBookLibraryDbContext();
            _employeeTypeRepository = new EmployeeTypeRepository(db);
        }        
        // GET: Manage/EmployeeTypes
        [Route("danh-sach-loai-nhan-vien")]
        public ActionResult Index()
        {
            if (ManageSession.HasRole("VIEW_EMPLOYEETYPE"))
            {
                return View(_employeeTypeRepository.GetAll());
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền xem loại nhân viên!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }
        
        // GET: Manage/EmployeeTypes/Create
        public ActionResult Create()
        {
            if (ManageSession.HasRole("CREATE_EMPLOYEETYPE"))
            {
                return View();
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm loại nhân viên!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }        
        [HttpPost]
        public ActionResult Create(EmployeeType employeeType)
        {
            if (ManageSession.HasRole("CREATE_EMPLOYEETYPE"))
            {
                if (employeeType.EmployeeTypeName == "")
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = false,
                        Message = "Không được để trống tên loại nhân viên!",
                    };
                }
                else
                {
                    var temp = db.EmployeeTypes.ToList().FirstOrDefault(x => x.EmployeeTypeName.Equals(employeeType.EmployeeTypeName));
                    if (temp != null)
                    {
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = false,
                            Message = "Tên loại đã tồn tại!",
                        };
                    }
                    else
                    {
                        db.EmployeeTypes.Add(employeeType);
                        db.SaveChanges();
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = true,
                            Message = "Thêm thành công!",
                        };
                        return RedirectToAction("Index");
                    }
                }
                return View(employeeType);
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền thêm loại nhân viên!",
                };
                return RedirectToAction("Index", "ManageHome", new { Area = "Manage" });
            }
        }        
        public ActionResult Edit(int? id)
        {
            if (ManageSession.HasRole("EDIT_EMPLOYEETYPE"))
            {
                if (id == null)
                {
                    return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
                }
                EmployeeType employeeType = db.EmployeeTypes.Find(id);
                if (employeeType == null)
                {
                    return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
                }
                if (employeeType.IsAdministrator == true)
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = false,
                        Message = "Không được xóa loại Administrator",
                    };
                    return RedirectToAction("Index");
                }
                return View(employeeType);
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền chỉnh sửa loại nhân viên!",
                };
                return RedirectToAction("Index", "EmployeeTypes", new { Area = "Manage" });
            }
        }
        
        [HttpPost]
        public ActionResult Edit(EmployeeType employeeType)
        {
            if (ManageSession.HasRole("EDIT_EMPLOYEETYPE"))
            {
                if (employeeType.IsAdministrator == true)
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = false,
                        Message = "Không được xóa loại Administrator",
                    };
                    return RedirectToAction("Index");
                }
                else if (employeeType.EmployeeTypeName == "")
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = false,
                        Message = "Không được để trống tên loại nhân viên!",
                    };
                }
                else
                {
                    var temp = db.EmployeeTypes.ToList().FirstOrDefault(x => x.EmployeeTypeName.Equals(employeeType.EmployeeTypeName) && x.EmployeeTypeId != employeeType.EmployeeTypeId);
                    if (temp != null)
                    {
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = false,
                            Message = "Tên loại đã tồn tại!",
                        };
                    }
                    else
                    {
                        _employeeTypeRepository.Update(employeeType);
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = true,
                            Message = "Sửa thành công!",
                        };
                        return RedirectToAction("Index");
                    }
                }
                return View(employeeType);
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền chỉnh sửa loại nhân viên!",
                };
                return RedirectToAction("Index", "EmployeeTypes", new { Area = "Manage" });
            }
        }
        
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (ManageSession.HasRole("DELETE_EMPLOYEETYPE"))
            {
                EmployeeType employeeType = db.EmployeeTypes.Find(id);
                if (employeeType != null)
                {
                    if (employeeType.IsAdministrator == true)
                    {
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = false,
                            Message = "Không được xóa loại Administrator",
                        };
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.EmployeeTypes.Remove(employeeType);
                        db.SaveChanges();
                        TempData["Notify"] = new JsonResultBO()
                        {
                            Status = true,
                            Message = "Xóa thành công!",
                        };
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = false,
                        Message = "Không tồn tại!",
                    };
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Bạn không có quyền xóa loại nhân viên!",
                };
                return RedirectToAction("Index", "EmployeeTypes", new { Area = "Manage" });
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
