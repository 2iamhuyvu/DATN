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
    public class EmployeeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeTypeId { get; set; }
        [Required(ErrorMessage ="Không được để trống tên loại nhân viên!")]
        public string EmployeeTypeName { get; set; }
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool? IsAdministrator { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public ICollection<Role_EmployeeType> Role_EmployeeTypes { get; set; }
    }
}
