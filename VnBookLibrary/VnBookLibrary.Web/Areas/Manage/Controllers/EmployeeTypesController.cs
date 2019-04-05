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
    public class EmployeeTypesController : Controller
    {
        private VnBookLibraryDbContext db;
        private UnitOfWork UoW;
        public EmployeeTypesController()
        {
            db = new VnBookLibraryDbContext();
            UoW = new UnitOfWork(db);
        }
        [HasRole(RoleCode = "VIEW_EMPLOYEETYPE")]
        public ActionResult Index()
        {
            return View(UoW.EmployeeTypeRepository.GetAll());
        }

        [HasRole(RoleCode = "CREATE_EMPLOYEETYPE")]
        public ActionResult Create()
        {
            ViewBag.ListGroupRole = UoW.GroupRoleRepository.GetAll();
            return View();
        }
        [HasRole(RoleCode = "CREATE_EMPLOYEETYPE")]
        [HttpPost]
        public ActionResult Create(EmployeeType employeeType)
        {
            var ListRoleCode = Request.Form.GetValues("ListRoleCode");
            if (ModelState.IsValid)
            {
                var temp = UoW.EmployeeTypeRepository.GetAll().FirstOrDefault(x => x.EmployeeTypeName.Equals(employeeType.EmployeeTypeName));
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
                    UoW.EmployeeTypeRepository.Insert(employeeType);
                    if (ListRoleCode != null)
                    {
                        ICollection<Role_EmployeeType> role_EmployeeTypes = new List<Role_EmployeeType>();
                        foreach (var rolecode in ListRoleCode)
                        {
                            Role_EmployeeType re = new Role_EmployeeType
                            {
                                RoleCode = rolecode,
                                EmployeeTypeId = employeeType.EmployeeTypeId,
                            };
                            role_EmployeeTypes.Add(re);
                        }
                        UoW.Role_EmployeeTypeRepository.InsertRange(role_EmployeeTypes);
                    }
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Thêm thành công!",
                    };
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ListGroupRole = UoW.GroupRoleRepository.GetAll();
            return View(employeeType);
        }

        [HasRole(RoleCode = "EDIT_EMPLOYEETYPE")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            EmployeeType employeeType = UoW.EmployeeTypeRepository.Find(id);
            if (employeeType == null)
            {
                return View("~/Areas/Manage/Views/Shared/_BadRequest.cshtml");
            }
            if (employeeType.IsAdministrator == true)
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Không được chỉnh Sửa loại Administrator",
                };
                return RedirectToAction("Index");
            }
            ViewBag.ListGroupRole = UoW.GroupRoleRepository.GetAll();
            ViewBag.HasRole = UoW.Role_EmployeeTypeRepository.GetAll().Where(x => x.EmployeeTypeId == id).Select(x => x.RoleCode).ToList();
            return View(employeeType);
        }

        [HasRole(RoleCode = "EDIT_EMPLOYEETYPE")]
        [HttpPost]
        public ActionResult Edit(EmployeeType employeeType)
        {
            var ListRoleCode = Request.Form.GetValues("ListRoleCode").ToList();
            if (employeeType.IsAdministrator == true)
            {
                TempData["Notify"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Không được sửa loại Administrator",
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
                var temp = UoW.EmployeeTypeRepository.GetAll().FirstOrDefault(x => x.EmployeeTypeName.Equals(employeeType.EmployeeTypeName) && x.EmployeeTypeId != employeeType.EmployeeTypeId);
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
                    var temp1 = db.EmployeeTypes.Find(employeeType.EmployeeTypeId);
                    temp1.EmployeeTypeName = employeeType.EmployeeTypeName;
                    temp1.Description = employeeType.Description;
                    db.SaveChanges();
                    UoW.EmployeeTypeRepository.ChangeRole(employeeType.EmployeeTypeId, ListRoleCode);
                    TempData["Notify"] = new JsonResultBO()
                    {
                        Status = true,
                        Message = "Sửa thành công!",
                    };
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ListGroupRole = UoW.GroupRoleRepository.GetAll();
            return View(employeeType);
        }
        [HasRole(RoleCode = "DELETE_EMPLOYEETYPE")]
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (ManageSession.HasRole("DELETE_EMPLOYEETYPE"))
            {
                EmployeeType employeeType = UoW.EmployeeTypeRepository.Find(id);
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
                        UoW.EmployeeTypeRepository.Delete(id);
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
            TempData["Notify"] = new JsonResultBO()
            {
                Status = false,
                Message = "Bạn không có quyền này!",
            };
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
