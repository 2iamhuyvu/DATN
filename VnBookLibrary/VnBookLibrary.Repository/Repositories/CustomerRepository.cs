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
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public CustomerRepository() : base()
        {
        }
        public List<Product> GetPurchasedProducts(int customerId)
        {
            List<Product> rs = new List<Product>();
            List<Bill> bills = _context.Bills.Where(x => x.CustomerId == customerId).ToList();
            if (bills != null && bills.Count > 0)
            {
                foreach (var bill in bills)
                {
                    var billDetais = bill.BillDetails;
                    foreach (var item in billDetais)
                    {
                        rs.Add(item.Product);
                    }
                }
            }
            return rs;
        } 
        public Customer LoginFacebook(Customer customer)
        {
            var c = _context.Customers.FirstOrDefault(x => x.FacebookId.Equals(customer.FacebookId));
            if (c == null)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }
            return customer;
        }
    }
}
