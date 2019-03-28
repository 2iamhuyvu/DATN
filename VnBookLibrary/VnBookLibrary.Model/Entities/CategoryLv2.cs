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
    public class CategoryLv2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryLv2Id { get; set; }

        [Display(Name = "Tên danh mục cấp 2")]
        [Required(ErrorMessage = "Không được để trống tên danh mục!")]
        public string CategoryLv2Name { get; set; }

        [Display(Name = "Danh mục cấp 1(danh mục cha)")]
        [Required(ErrorMessage = "Chọn danh mục cấp 1 là bắt buộc!")]
        public int CategoryLv1Id { get; set; }
        [ForeignKey("CategoryLv1Id")]
        public virtual CategoryLv1 CategoryLv1 { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
