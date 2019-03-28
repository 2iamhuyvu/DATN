using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VnBookLibrary.Model.Entities
{
    public class Ward
    {
        [Key]
        public int WardId { get; set; }
        public string WardName { get; set; }
        public int DidtrictId { get; set; }

        [ForeignKey("DidtrictId")]
        public virtual District District { get; set; }        
    }
}