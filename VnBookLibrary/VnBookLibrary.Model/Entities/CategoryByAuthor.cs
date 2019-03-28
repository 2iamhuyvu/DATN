using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class CategoryByAuthor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryAuthorId { get; set; }

        [Display(Name ="Tên tác giả")]
        [Required(ErrorMessage ="Không được để trống tên danh mục!")]
        public string CategoryAuthorName { get; set; }
        [Display(Name = "Thông tin tác giả")]
        [Column(TypeName ="NTEXT")]
        public string DescriptionAuthor { get; set; }        
        public virtual ICollection<Product> Products { get; set; }
    }
}
