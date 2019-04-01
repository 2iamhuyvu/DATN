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
    public class RateProduct
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int CustomerId { get; set; }
        [DefaultValue(1)]
        public int? Point { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
