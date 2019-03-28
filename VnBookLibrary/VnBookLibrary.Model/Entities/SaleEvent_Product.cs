using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace VnBookLibrary.Model.Entities
{
    public class SaleEvent_Product
    {
        [Key,Column(Order =0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int SaleEventId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("SaleEventId")]
        public virtual SaleEvent SaleEvent { get; set; }
    }
}
