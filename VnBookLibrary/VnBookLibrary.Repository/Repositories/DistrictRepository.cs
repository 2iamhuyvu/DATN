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
    public class DistrictRepository : GenericRepository<District>
    {
        public DistrictRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public DistrictRepository() : base()
        {
        }       
    }
}
