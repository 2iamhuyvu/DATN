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
    public class SaleEvent_ProductRepository: GenericRepository<SaleEvent_Product>
    {
        public SaleEvent_ProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public SaleEvent_ProductRepository() : base()
        {
        }       
    }
}
