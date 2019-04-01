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
    public class RateProductRepository : GenericRepository<RateProduct>
    {
        public RateProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public RateProductRepository() : base()
        {
        }
        public int Delete(int ProductId, int CustomerId)
        {
            RateProduct rateProduct = _context.RateProducts.FirstOrDefault(x => x.ProductId == ProductId && x.CustomerId == CustomerId);
            if (rateProduct != null)
                _context.RateProducts.Remove(rateProduct);
            return _context.SaveChanges();
        }
    }
}
