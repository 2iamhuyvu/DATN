using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class Tag_Product
    {
        [Key, Column(Order = 0)]
        public int TagId { get; set; }

        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

