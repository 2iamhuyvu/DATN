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
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required(ErrorMessage ="Không được để trống tên!")]
        [Display(Name ="Tên khách hàng")]
        public string CustomerName { get; set; }      
        
        [Required(ErrorMessage ="Không được để trống Email!")]
        [EmailAddress]        
        public string Email { get; set; } 
        
        [Display(Name ="Điện thoại")]
        [Required(ErrorMessage ="Không được để trống điện thoại!")]
        [RegularExpression("^0{1}[1-9]{1}[0-9]{8}$",ErrorMessage ="Không đúng định dạng số điện thoại!")]
        public string Phone { get; set; }

        [Display(Name ="Tên đăng nhập")]        
        [Required(ErrorMessage ="Không được để trống tên đăng nhập!")]        
        public string LoginName { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "Không được để trống mật khẩu!")]
        [MinLength(5, ErrorMessage = "Mật khẩu tối thiểu là 5 ký tự")]
        public string Password { get; set; }
        
        [NotMapped]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng!")]        
        public string RePassword { get; set; }        
        [DefaultValue(false)]
        public bool? IsBlock { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<LikeProduct> LikeProducts { get; set; }
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        public virtual ICollection<RateProduct> RateProducts { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
