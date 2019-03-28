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
    public class GroupRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupRoleId { get; set; }
        [Required(ErrorMessage ="Không được để trống tên nhóm!")]
        public string GroupRoleName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
