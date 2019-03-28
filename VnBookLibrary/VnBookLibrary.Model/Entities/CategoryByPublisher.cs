using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class CategoryByPublisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryByPublisherId { get; set; }
        [Display(Name = "Tên NXB - NPH")]
        [Required(ErrorMessage = "Không được để trống tên danh mục!")]        
        public string CategoryByPublisherName { get; set; }

        [Display(Name = "Thông tin NXB - NPH")]
        [Column(TypeName ="NTEXT")]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
