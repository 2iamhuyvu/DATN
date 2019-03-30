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
    }
}
