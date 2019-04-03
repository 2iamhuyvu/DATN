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
    public class Tag_ProductRepository: GenericRepository<Tag_Product>
    {
        public Tag_ProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public Tag_ProductRepository() : base()
        {
        }       
    }
}
