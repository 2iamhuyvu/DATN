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
    public class BillDetail
    {
        [Key,Column(Order =0)]
        public int BillId { get; set; }
        [Key,Column(Order =1)]
        public int ProductId { get; set; }
        [DefaultValue(1)]
        public int? Quantity { get; set; }
        [ForeignKey("BillId")]
        public virtual Bill Bill { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }        
    }
}
