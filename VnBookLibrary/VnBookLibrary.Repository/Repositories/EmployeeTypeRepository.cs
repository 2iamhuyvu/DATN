using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;

namespace VnBookLibrary.Repository.Repositories
{
    public class EmployeeTypeRepository : GenericRepository<EmployeeType>
    {
        public EmployeeTypeRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public EmployeeTypeRepository() : base()
        {
        }
        public void ChangeRole(int employeeTypeId, List<string> roleCodes)
        {
            List<Role_EmployeeType> role_EmployeeTypes = _context.Role_EmployeeTypes.Where(x => x.EmployeeTypeId == employeeTypeId).ToList();
            if (role_EmployeeTypes != null)
            {
                _context.Role_EmployeeTypes.RemoveRange(role_EmployeeTypes);
                _context.SaveChanges();
            }
            if (roleCodes != null)
            {
                foreach (var roleCode in roleCodes)
                {
                    Role_EmployeeType re = new Role_EmployeeType()
                    {
                        EmployeeTypeId = employeeTypeId,
                        RoleCode = roleCode,
                    };
                    _context.Role_EmployeeTypes.Add(re);
                }
            }
            _context.SaveChanges();
        }
    }
}
