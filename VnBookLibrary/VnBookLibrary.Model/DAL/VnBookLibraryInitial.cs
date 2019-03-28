using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.Entities;

namespace VnBookLibrary.Model.DAL
{
    public class VnBookLibraryInitial : DropCreateDatabaseIfModelChanges<VnBookLibraryDbContext>
    {
        protected override void Seed(VnBookLibraryDbContext context)
        {           
            var d = System.AppContext.BaseDirectory;
            context.Database.ExecuteSqlCommand(File.ReadAllText(d+"\\Data\\data.sql"));            
        }
    }
}
