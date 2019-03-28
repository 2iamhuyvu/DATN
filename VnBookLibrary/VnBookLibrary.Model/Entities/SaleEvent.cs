using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class SaleEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleEventId { get; set; }
        public string SaleEventName { get; set; }
        [Column(TypeName ="NTEXT")]
        public string Descripton { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [DefaultValue(0)]
        public int? Percent { get; set; }
        public virtual ICollection<SaleEvent_Product> SaleEvent_Products { get; set; }
    }
}
