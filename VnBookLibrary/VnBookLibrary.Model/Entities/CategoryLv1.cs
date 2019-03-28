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
    public class CategoryLv1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryLv1Id { get; set; }
        [Display(Name ="Tên danh mục cấp 1")]
        [Required(ErrorMessage ="Không được để trống tên danh mục")]
        public string CategoryLv1Name { get; set; }
        [DefaultValue(1)]
        [Display(Name ="Thứ tự hiển thị")]
        public int? OrderDisplay { get; set; }        
        public virtual ICollection<CategoryLv2> CategoryLv2s { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
