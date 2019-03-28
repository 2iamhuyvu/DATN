using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Web.Areas.Manage.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VnBookLibrary.Web.Areas.Manage.Controllers
{    
    public class LoginController : Controller
    {
        #region khai báo;        
        private EmployeeRepository _employeeRepository;
        #endregion
        public LoginController()
        {
            _employeeRepository = new EmployeeRepository();
        }
        [HttpGet]        
        public ActionResult Index(string returnURL)
        {                      
            Session[Constants.MANAGE_SESSION]= null;
            if (Url.IsLocalUrl(returnURL) && !string.IsNullOrEmpty(returnURL))
            {
                ViewBag.ReturnURL = returnURL;
            }            
            return View();
        }
        [HttpPost]
        public ActionResult Login(string TenDangNhap, string MatKhau, string returnURL)
        {
            Employee employee = new Employee();
            int check = _employeeRepository.CheckLogin(TenDangNhap, MatKhau, out employee);
            if (check == 1)
            {
                Session[Constants.MANAGE_SESSION] = new ManageSessionModel()
                {
                    SessionEmployee = employee,
                    RoleCodes = _employeeRepository.ListRoleCodeByEmployee(employee.EmployeeId)
                };                  
                TempData["NotifyLogin"] = new JsonResultBO()
                {
                    Status = true,
                    Message = "Đăng nhập thành công!"
                };
                if (Url.IsLocalUrl(returnURL))
                {
                    return Redirect(returnURL);
                }                
                return RedirectToAction("Index", "Employees", new { area = "Manage" });
            }
            else if(check==-1)
            {
                Session[Constants.MANAGE_SESSION] = null;                
                TempData["NotifyLogin"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Tài khoản bị khóa"
                };
                return RedirectToAction("Index", "Login", new { area = "Manage" });
            }
            else
            {
                Session[Constants.MANAGE_SESSION] = null;                
                TempData["NotifyLogin"] = new JsonResultBO()
                {
                    Status = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                };
                return RedirectToAction("Index", "Login", new { area = "Manage" });
            }
        }
        [HttpPost]
        public ActionResult Logout()
        {
            Session[Constants.MANAGE_SESSION] = null;            
            return RedirectToAction("Index", "Login", new { area = "Manage" });
        }

        public PartialViewResult _SessionEnd()
        {
            return PartialView();
        }
        public PartialViewResult _NoRole(string roleName)
        {
            ViewBag.RoleName = roleName;
            return PartialView();
        }
    }
}