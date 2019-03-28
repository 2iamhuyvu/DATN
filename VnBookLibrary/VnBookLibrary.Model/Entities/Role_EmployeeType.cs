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
    public class Role_EmployeeType
    {
        [Key, Column(Order = 0)]
        public string RoleCode { get; set; }
        [Key, Column(Order = 1)]
        public int EmployeeTypeId { get; set; }
        [ForeignKey("RoleCode")]
        public virtual Role Role{get;set;}
        [ForeignKey("EmployeeTypeId")]
        public virtual EmployeeType EmployeeType { get; set; }
    }
}
