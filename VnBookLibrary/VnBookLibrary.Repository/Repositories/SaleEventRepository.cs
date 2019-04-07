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
    public class SaleEventRepository : GenericRepository<SaleEvent>
    {
        public SaleEventRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public SaleEventRepository() : base()
        {
        }
        public int CheckLogin(string loginName, string password, out Employee employee)
        {
            employee = null;
            Employee tempEmployee = _context.Employees.FirstOrDefault(nv => nv.LoginName.Equals(loginName) && nv.Password.Equals(PasswordEncryption.GetVnBookLibraryCode(password)));            
            if (tempEmployee != null)
            {
                if (tempEmployee.IsBlock== true)
                {
                    return -1;
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
