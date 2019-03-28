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
    public class CategoryLv2Repository : GenericRepository<CategoryLv2>
    {
        public CategoryLv2Repository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public CategoryLv2Repository() : base()
        {
        }      
    }
}
