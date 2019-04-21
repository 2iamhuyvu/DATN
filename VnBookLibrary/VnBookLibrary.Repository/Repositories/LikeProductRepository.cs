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
    public class LikeProductRepository : GenericRepository<LikeProduct>
    {
        public LikeProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public LikeProductRepository() : base()
        {
        }
        public int Delete(int ProductId,int CustomerId)
        {
            LikeProduct likeProduct = _context.LikeProducts.FirstOrDefault(x => x.ProductId == ProductId && x.CustomerId == CustomerId);
            if (likeProduct != null)
                _context.LikeProducts.Remove(likeProduct);
            return _context.SaveChanges();
        }
        public List<Product>GetProductsBeLiked(int customerId)
        {
            return _context.LikeProducts.Where(x => x.CustomerId == customerId).ToList().Select(x => x.Product).ToList();
        }
    }
}
