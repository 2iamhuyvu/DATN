using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VnBookLibrary.Model.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Display(Name="Tên nhân viên")]
        [Required(ErrorMessage = "Không được để trống tên nhân viên!")]        
        public string EmployeeName { get; set; }
        
        [EmailAddress(ErrorMessage ="Không đúng định dạng email")]
        //[RegularExpression(@"^[a-z][a-z0-9_\.]{5,32}@[a-z0-9]{2,}(\.[a-z0-9]{2,4}){1,2}$",ErrorMessage ="Không đúng định dạng email")]
        public string Email { get; set; }
        
        [RegularExpression(@"^0+[1-9]{1}[0-9]{8}$",ErrorMessage ="Không đúng định dạng số điện thoại!")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Không được để trống tên đăng nhập!")]        
        [Display(Name = "Tên đăng nhập")]
        public string LoginName { get; set; }
        
        [Required(ErrorMessage = "Không được để trống mật khẩu!")]        
        [Display(Name = "Mật khẩu")]        
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password",ErrorMessage ="Xác nhận mật khẩu không đúng!")]        
        [NotMapped]
        public string RePassword { get; set; }

        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateBirth { get; set; }
        [DefaultValue(false)]

        [Display(Name = "Bị khóa")]
        public bool IsBlock { get; set; }

        [Display(Name = "Loại nhân viên")]
        [Required(ErrorMessage = "Không được để loại nhân viên!")]
        public int EmployeeTypeId { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }

        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
