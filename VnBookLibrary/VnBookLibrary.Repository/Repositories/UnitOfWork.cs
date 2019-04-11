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

namespace VnBookLibrary.Repository.Repositories
{
    public class UnitOfWork
    {
        private VnBookLibraryDbContext _context;
        private DbContextTransaction transaction;
        public UnitOfWork()
        {
            _context = new VnBookLibraryDbContext();
            EmployeeRepository = new EmployeeRepository(_context);
            EmployeeTypeRepository = new EmployeeTypeRepository(_context);
            BillRepository = new BillRepository(_context);
            BillDetailRepository = new BillDetailRepository(_context);
            CategoryByAuthorRepository = new CategoryByAuthorRepository(_context);
            CategoryByPublisherRepository = new CategoryByPublisherRepository(_context);
            CategoryLv1Repository = new CategoryLv1Repository(_context);
            CategoryLv2Repository = new CategoryLv2Repository(_context);
            CommentProductRepository = new CommentProductRepository(_context);
            ContactRepository = new ContactRepository(_context);
            CustomerRepository = new CustomerRepository(_context);
            DistrictRepository = new DistrictRepository(_context);
            GroupRoleRepository = new GroupRoleRepository(_context);
            LikeProductRepository = new LikeProductRepository(_context);
            NewsRepository = new NewsRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ProvinceRepository = new ProvinceRepository(_context);
            RateProductRepository = new RateProductRepository(_context);
            Role_EmployeeTypeRepository = new Role_EmployeeTypeRepository(_context);
            RoleRepository = new RoleRepository(_context);
            //SaleEventRepository = new SaleEventRepository(_context);
            //SaleEvent_ProductRepository = new SaleEvent_ProductRepository(_context);
            WardRepository = new WardRepository(_context);
        }
        public UnitOfWork(VnBookLibraryDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
            EmployeeTypeRepository = new EmployeeTypeRepository(_context);
            BillRepository = new BillRepository(_context);
            BillDetailRepository = new BillDetailRepository(_context);
            CategoryByAuthorRepository = new CategoryByAuthorRepository(_context);
            CategoryByPublisherRepository = new CategoryByPublisherRepository(_context);
            CategoryLv1Repository = new CategoryLv1Repository(_context);
            CategoryLv2Repository = new CategoryLv2Repository(_context);
            CommentProductRepository = new CommentProductRepository(_context);
            ContactRepository = new ContactRepository(_context);
            CustomerRepository = new CustomerRepository(_context);
            DistrictRepository = new DistrictRepository(_context);
            GroupRoleRepository = new GroupRoleRepository(_context);
            LikeProductRepository = new LikeProductRepository(_context);
            NewsRepository = new NewsRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ProvinceRepository = new ProvinceRepository(_context);
            RateProductRepository = new RateProductRepository(_context);
            Role_EmployeeTypeRepository = new Role_EmployeeTypeRepository(_context);
            RoleRepository = new RoleRepository(_context);
            //SaleEventRepository = new SaleEventRepository(_context);
            //SaleEvent_ProductRepository = new SaleEvent_ProductRepository(_context);
            WardRepository = new WardRepository(_context);
            RecommendRepository = new RecommendRepository(_context);
            TagRepository = new TagRepository(_context);
            Tag_ProductRepository = new Tag_ProductRepository(_context);
        }

        public RecommendRepository RecommendRepository { get; set; }
        public TagRepository TagRepository { get; set; }
        public Tag_ProductRepository Tag_ProductRepository { get; set; }
        public EmployeeRepository EmployeeRepository { get; set; }
        public EmployeeTypeRepository EmployeeTypeRepository { get; set; }
        public BillDetailRepository BillDetailRepository { get; set; }
        public BillRepository BillRepository { get; set; }
        public CategoryLv1Repository CategoryLv1Repository { get; set; }
        public CategoryLv2Repository CategoryLv2Repository { get; set; }
        public CategoryByPublisherRepository CategoryByPublisherRepository { get; set; }
        public CategoryByAuthorRepository CategoryByAuthorRepository { get; set; }
        public CommentProductRepository CommentProductRepository { get; set; }
        public ContactRepository ContactRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public DistrictRepository DistrictRepository { get; set; }
        public GroupRoleRepository GroupRoleRepository { get; set; }
        public LikeProductRepository LikeProductRepository { get; set; }
        public NewsRepository NewsRepository { get; set; }
        public ProductRepository ProductRepository { get; set; }
        public ProvinceRepository ProvinceRepository { get; set; }
        public RateProductRepository RateProductRepository { get; set; }
        public Role_EmployeeTypeRepository Role_EmployeeTypeRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        //public SaleEventRepository SaleEventRepository { get; set; }
        //public SaleEvent_ProductRepository SaleEvent_ProductRepository { get; set; }
        public WardRepository WardRepository { get; set; }
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
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
