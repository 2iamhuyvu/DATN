using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.DAL;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Commons;
using PagedList;
using PagedList.Mvc;
namespace VnBookLibrary.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public ProductRepository() : base()
        {
        }        
        public IPagedList<Product> GetPageListProduct(string search, int? categoryLv1Id, int? categoryLv2Id, int? categoryAuthorId, int? categoryPublisherId,int pageNumber = 1, int pageSize = 10)
        {
            var list = _context.Products.ToList();
            if(search!=null&&search!="")
            {
                list = list.Where(x => x.ProductName.ToUpper().Contains(search.ToUpper())).ToList();
            }
            if (categoryLv1Id > 0)
            {
                list = list.Where(x => x.CategoryLv1Id == categoryLv1Id).ToList();
            }
            if (categoryLv2Id > 0)
            {
                list = list.Where(x => x.CategoryLv2Id == categoryLv2Id).ToList();
            }
            if (categoryAuthorId > 0)
            {
                list = list.Where(x => x.CategoryByAuthorId == categoryAuthorId).ToList();
            }
            if (categoryPublisherId > 0)
            {
                list = list.Where(x => x.CategoryByPublisherId == categoryPublisherId).ToList();
            }
            return list.OrderBy(x=>x.CategoryLv1Id).ThenBy(x=>x.CategoryLv2Id).ThenBy(x=>x.ProductName).ToPagedList(pageNumber, pageSize);
        }
    }
}
