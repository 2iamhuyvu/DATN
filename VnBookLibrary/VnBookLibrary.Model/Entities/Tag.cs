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
        public string TagId { get; set; }

        [Required]
        public string TagName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Tag_Product> Tag_Products { get; set; }
    }
}
