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
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public CustomerRepository() : base()
        {
        }        
    }
}
