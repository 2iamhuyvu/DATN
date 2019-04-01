using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnBookLibrary.Model.Entities
{
    public class CommentProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentProductId { get; set; }
        public int ProductId { get; set; }        
        public int CustomerId { get; set; }
        [Column(TypeName ="NTEXT")]
        public string Comments { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CommentDate { get; set; }
        [DefaultValue(false)]
        public bool? AlowDisplay { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
