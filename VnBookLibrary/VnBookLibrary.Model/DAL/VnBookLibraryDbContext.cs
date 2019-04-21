using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnBookLibrary.Model.Entities;
namespace VnBookLibrary.Model.DAL
{
    public class VnBookLibraryDbContext:DbContext
    {
        public VnBookLibraryDbContext() : base("ConnectionString")
        {
            Database.SetInitializer<VnBookLibraryDbContext>(new VnBookLibraryInitial());
        }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<Role> Roles { get; set; }        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Bill> Bills { get; set; }
        //public DbSet<SaleEvent> SaleEvents { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<CategoryLv1> CategoryLv1s { get; set; }
        public DbSet<CategoryLv2> CategoryLv2s { get; set; }
        public DbSet<CategoryByAuthor> CategoryByAuthors { get; set; }
        public DbSet<CategoryByPublisher> CategoryByPublishers { get; set; }
        public DbSet<Product> Products { get; set; }        
        public DbSet<LikeProduct> LikeProducts { get; set; }
        public DbSet<CommentProduct> CommentProducts { get; set; }
        public DbSet<RateProduct> RateProducts { get; set; }
        public DbSet<Role_EmployeeType> Role_EmployeeTypes { get; set; }
        //public DbSet<SaleEvent_Product> SaleEvent_Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Tag_Product> Tag_Products { get; set; }
        public DbSet<Recommend> Recommends { get; set; }
    }
}
