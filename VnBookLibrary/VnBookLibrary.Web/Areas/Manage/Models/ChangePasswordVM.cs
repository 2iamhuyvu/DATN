using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VnBookLibrary.Web.Areas.Manage.Customizes;

namespace VnBookLibrary.Web.Areas.Manage.Models
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage ="Không được để trống!")]
        [NotNull(ErrorMessage = "Không được để mật khẩu là các ký tự trống!")]
        [Display(Name ="Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Không được để trống!")]
        [NotNull(ErrorMessage = "Không được để mật khẩu là các ký tự trống!")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối tiểu 5 ký tự!")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [NotNull(ErrorMessage = "Không được để mật khẩu là các ký tự trống!")]
        [Compare("NewPassword",ErrorMessage ="Xác nhận mật khẩu không đúng!")]
        [Required(ErrorMessage = "Không được để trống!")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối tiểu 5 ký tự!")]
        public string ConfirmPassword { get; set; }
    }
}
