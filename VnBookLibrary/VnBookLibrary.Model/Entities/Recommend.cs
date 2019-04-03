using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class Recommend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecommendId { get; set; }

        public int? ProductId1 { get; set; }
        
        public int? ProductId2 { get; set; }        
        public int Count { get; set; }

        [ForeignKey("ProductId1")]
        public virtual Product Product1 { get; set; }

        [ForeignKey("ProductId2")]
        public virtual Product Product2 { get; set; }
    }
}
