using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using VnBookLibrary.Model.Entities;
using VnBookLibrary.Repository.Repositories;
using VnBookLibrary.Model.DAL;

namespace BOOKSHOP.Business.BaseRepository
{
    public class UnitOfWork
    {
        private VnBookLibraryDbContext _context;       
        public UnitOfWork()
        {
            _context = new VnBookLibraryDbContext();
            EmployeeRepository = new EmployeeRepository(_context);
            EmployeeTypeRepository = new EmployeeTypeRepository(_context);
        }
        public EmployeeRepository EmployeeRepository { get;private set; }
        public EmployeeTypeRepository EmployeeTypeRepository { get; private set; }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
 
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
