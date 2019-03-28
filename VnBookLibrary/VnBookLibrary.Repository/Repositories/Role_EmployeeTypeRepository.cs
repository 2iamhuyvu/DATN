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
    public class Role_EmployeeTypeRepository : GenericRepository<Role_EmployeeType>
    {
        public Role_EmployeeTypeRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public Role_EmployeeTypeRepository() : base()
        {
        }        
    }
}
