using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class Tag
    {
        [Key]                
        public int TagId { get; set; }

        [Required(ErrorMessage ="Không được để trống tên Tag")]
        public string TagName { get; set; }

        public string Description { get; set; }

        public bool IsDisplay { get; set; }
        public int? OrderDisplay { get; set; }
        public virtual ICollection<Tag_Product> Tag_Products { get; set; }
    }
}
