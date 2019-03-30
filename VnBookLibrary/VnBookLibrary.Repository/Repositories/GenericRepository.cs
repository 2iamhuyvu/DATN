using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.DAL;

namespace VnBookLibrary.Repository.Repositories
{
    public class GenericRepository<T>where T:class
    {
        protected VnBookLibraryDbContext _context;
        private DbSet<T> _set;
        private DbContextTransaction transaction;
        public GenericRepository(VnBookLibraryDbContext context)
        {
            if (context == null)
                context = new VnBookLibraryDbContext();
            _context = context;
            _set = context.Set<T>();
        }
        public GenericRepository()
        {
            _context = new VnBookLibraryDbContext();
        }        
        public virtual T Find(object id)
        {
            return _set.Find(id);
        }
        public virtual int Insert(T item)
        {
            _set.Add(item);
            return _context.SaveChanges();
        }
        public virtual int InsertRange(ICollection<T> listItem)
        {
            _set.AddRange(listItem);
            return _context.SaveChanges();
        }
        public virtual int Delete(object id)
        {
            var item = Find(id);
            if (item != null)
            {
                _set.Remove(item);
                return _context.SaveChanges();
            }
            return -1;
        }
        public virtual int MultiDelete(List<Object> ListId)
        {
            int rs = 0;
            foreach (var id in ListId)
            {
                if (Delete(id) > 0)
                {
                    rs++;
                }
            }
            return rs;
        }        
        public virtual int Update(T item)
        {
            _set.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            return _context.SaveChanges();
        }       
        public virtual ICollection<T> GetAll()
        {
            return _set.ToList();
        }
        public virtual T GetByID(object id)
        {
            return _set.Find(id);
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public void BeginTransaction()
        {
            if (transaction == null)
                transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
                transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
                transaction.Rollback();
        }
    }
}
