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
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillId { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BuyDate { get; set; }
        public decimal? TotalNotSale { get; set; }
        public decimal? IntoMoney { get; set; }
        [DefaultValue(0)]
        public decimal? FeeShip { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PayDate { get; set; }
        [DefaultValue(0)]
        public int? Status { get; set; }
        public string Description { get; set; }
        public int? SaleEventId { get; set; }
        [ForeignKey("SaleEventId")]
        public  virtual SaleEvent SaleEvent { get; set; }
    }
}
