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
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        public DateTime ? PostDate { get; set; }        
        [Required(ErrorMessage ="Không được để trống tiêu đề!")]
        public string NewsTitle { get; set; }
        
        [Column(TypeName = "NTEXT")]
        public string NewsContent { get; set; }       
    }
}
