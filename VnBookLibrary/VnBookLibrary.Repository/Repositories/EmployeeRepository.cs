using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
namespace VnBookLibrary.Repository.Repositories
{
    public class EmployeeRepository:GenericRepository<Employee>
    {
        public EmployeeRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public EmployeeRepository() : base()
        {
        }
        public List<string> ListRoleCodeByEmployee (int employeeId) {
            Employee employee = _context.Employees.Find(employeeId);
            return employee.EmployeeType.Role_EmployeeTypes.Select(x => x.RoleCode).ToList();
        }       
        public int CheckLogin(string loginName, string password, out Employee employee)
        {
            employee = new Employee();
            Employee tempEmployee = _context.Employees.ToList().FirstOrDefault(nv => nv.LoginName.Equals(loginName) && nv.Password.Equals(PasswordEncryption.GetVnBookLibraryCode(password)));            
            if (tempEmployee != null)
            {
                if (tempEmployee.IsBlock == true)
                {
                    return -1;//Tài khoản bị khóa
                }
                else
                {
                    employee = tempEmployee;
                    return 1;
                }
            }
            return 0;
        }        
    }
}
