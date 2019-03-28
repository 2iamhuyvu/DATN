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
    public class Contact
    {
        public int ContactId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ContactContent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContactSendTime { get; set; }
        public int? CustomerId { get; set; }
        [DefaultValue(false)]
        public bool? IsReply { get; set; }
        public int? EmployeeId { get; set; }
        public string ReplyContent { get; set; }        
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee  Employee { get; set; }
    }
}
