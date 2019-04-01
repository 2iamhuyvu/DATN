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
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }  
        
        [Display(Name ="Tên sách - bộ sách")]
        [Required(ErrorMessage ="Không được để trống tên sách")]
        public string ProductName { get; set; }  
        
        [Display(Name ="Tình trạng")]
        [DefaultValue(1)]
        public int? Status { get; set; }

        [Display(Name ="Ngôn ngữ")]
        public string Language { get; set; }

        [Display(Name ="Số trang")]
        public int? PageNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Ngày xuất bản - Ngày phát hành")]
        public DateTime? PublishDate { get; set; }
        
        [Display(Name ="Thông tin sách - bộ sách")]
        [Column(TypeName ="NTEXT")]
        public string Description { get; set; }

        [Display(Name ="Ảnh sách")]
        public string AvatarLink { get; set; }

        [Display(Name ="Giá bán")]
        public decimal Price { get; set; }

        [Display(Name ="Giá bìa")]
        public decimal? CoverPrice { get; set; }

        [Display(Name ="Tác giả")]
        public int? CategoryByAuthorId { get; set; }
        [Display(Name ="Nhà xuất bản - Nhà phát hành")]
        public int? CategoryByPublisherId { get; set; }

        [Display(Name ="Danh mục cấp 2")]
        public int? CategoryLv2Id { get; set; }  
        [ForeignKey("CategoryLv2Id")]
        public virtual CategoryLv2 CategoryLv2 { get; set; }

        [Display(Name = "Danh mục cấp 1")]
        public int? CategoryLv1Id { get; set; }
        [ForeignKey("CategoryLv1Id")]
        public virtual CategoryLv1 CategoryLv1 { get; set; }

        [ForeignKey("CategoryByAuthorId")]
        public virtual CategoryByAuthor CategoryByAuthor { get; set; }
        [ForeignKey("CategoryByPublisherId")]
        public virtual CategoryByPublisher CategoryByPublisher { get; set; }
        public virtual ICollection<LikeProduct> LikeProducts { get; set; }
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        public virtual ICollection<RateProduct> RateProducts { get; set; }
        public virtual ICollection<SaleEvent_Product> SaleEvent_Products { get; set; }
    }
}
