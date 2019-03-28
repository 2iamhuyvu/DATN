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
    public class Role
    {
        [Key]
        [Column(TypeName ="VARCHAR")]
        [MaxLength(50)]
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }        
        public int? GroupRoleId { get; set; }
        [ForeignKey("GroupRoleId")]
        public virtual GroupRole GroupRole { get; set; }
        public ICollection<Role_EmployeeType> Role_EmployeeTypes { get; set; }
    }
}
