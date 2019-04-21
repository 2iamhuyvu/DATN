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
    public class RateProductRepository : GenericRepository<RateProduct>
    {
        public RateProductRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public RateProductRepository() : base()
        {
        }
        public List<Product> GetProductsBeRated(int customerId)
        {
            return _context.RateProducts.Where(x => x.CustomerId == customerId).ToList().Select(x => x.Product).ToList();
        }
        public List<ItemMatrix> GetMatrixRate()
        {
            var rs = new List<ItemMatrix>();
            var rates = _context.RateProducts.GroupBy(x => x.CustomerId);
            foreach (var rate in rates)
            {
                ItemMatrix itemMatrix = new ItemMatrix()
                {
                    CustomerId = rate.Key,

                };
                List<RateProduct> rateProducts = new List<RateProduct>();
                foreach (var item in rate)
                {
                    rateProducts.Add(item);
                }
                itemMatrix.RateProducts = rateProducts;
                rs.Add(itemMatrix);
            }
            return rs;
        }
        public async Task InsertByNewProduct(int productId)
        {
            var customers = _context.Customers;
            foreach (var c in customers)
            {
                RateProduct rateProduct = new RateProduct()
                {
                    CustomerId = c.CustomerId,
                    ProductId = productId,
                };
                _context.RateProducts.Add(rateProduct);
            }
            await _context.SaveChangesAsync();
        }
        public double predictRate(int customerId, int productId)
        {
            double tuso = 0;
            double mauso = 0;
            double averageCtm = AverageRateByCustomer(customerId);
            var temp1 = _context.RateProducts.Where(x => x.ProductId == productId && x.CustomerId != customerId).ToList();
            foreach (var item in temp1)
            {
                var averageCtm2 = AverageRateByCustomer(item.CustomerId);
                var a = Pearson(customerId, item.CustomerId);
                tuso =tuso+ Pearson(customerId, item.CustomerId) * (item.Point ?? 0 - averageCtm2);
                mauso =mauso+ Math.Abs(Pearson(customerId, item.CustomerId));
            }
            return (averageCtm + tuso / mauso);
        }
        public List<Product> GetSuggestions(int customerId, int numSuggestions)
        {
            List<ProductSuggestion> productsSuggest = new List<ProductSuggestion>();
            List<Product> rs = new List<Product>();
            var tempcus = _context.RateProducts.Where(x => x.CustomerId == customerId).ToList().Select(x => x.Product).ToList();
            var temp = _context.RateProducts.Where(x => x.CustomerId != customerId).ToList().Select(x => x.Product).ToList();
            foreach (var item in temp)
            {
                if (!tempcus.Contains(item))
                {
                    productsSuggest.Add(new ProductSuggestion()
                    {
                        Product = item,
                        PredictRate = predictRate(customerId, item.ProductId)
                    });
                }
            }
            var temp2 = productsSuggest.OrderByDescending(x => x.PredictRate).ToList().Skip(0).Take(numSuggestions).ToList();
            foreach (var item in temp2)
            {
                if (!checkExist(rs, item.Product))
                {
                    rs.Add(item.Product);
                }
            }
            return rs;
        }
        private bool checkExist(List<Product> rs, Product product)
        {
            if (rs == null)
                return false;
            if (rs.Contains(product))
            {
                return true;
            }
            return false;
        }
        public double Pearson(int customer1Id, int customer2Id)
        {
            var rateProductTogether = GetRateProductTogether(customer1Id, customer2Id);
            double averageCtm1 = AverageRateByCustomer(customer1Id);
            double averageCtm2 = AverageRateByCustomer(customer2Id);
            double numerator = 0;//tử số
            double temp1 = 0;
            double temp2 = 0;
            foreach (var item in rateProductTogether)
            {
                numerator += (item.PointCustomer1 - averageCtm1) * (item.PointCustomer2 - averageCtm2);
                temp1 += Math.Pow((item.PointCustomer1 - averageCtm1), 2);
                temp2 += Math.Pow((item.PointCustomer2 - averageCtm2), 2);
            }
            var denominator = Math.Sqrt(temp1) * Math.Sqrt(temp2);//mẫu số
            if (numerator == denominator) return 1;
            return numerator / denominator;
        }
        public double AverageRateByCustomer(int customerId)
        {
            try
            {
                var rates = _context.RateProducts.Where(x => x.CustomerId == customerId).ToList();
                double rs = 0;
                foreach (var r in rates)
                {
                    rs = rs + r.Point ?? 0;
                }
                rs = (double)(rs / rates.Count());
                return rs;
            }
            catch
            {
                return 0;
            }
        }
        public List<RateProductTogether> GetRateProductTogether(int customerId1, int customerId2)
        {
            List<RateProduct> RateCustomer1 = _context.RateProducts.Where(x => x.CustomerId == customerId1).ToList();
            List<RateProduct> RateCustomer2 = _context.RateProducts.Where(x => x.CustomerId == customerId2).ToList();            
            var a = (from r1 in RateCustomer1
                     join r2 in RateCustomer2 on r1.ProductId equals r2.ProductId 
                     select new RateProductTogether()
                     {
                         ProductId = r1.ProductId,
                         PointCustomer1 = r1.Point ?? 0,
                         PointCustomer2 = r2.Point ?? 0,
                     }).ToList();
            return a;
        }
        public async Task InsertByNewCustomer(int customerId)
        {
            var product = _context.Products;
            foreach (var p in product)
            {
                RateProduct rateProduct = new RateProduct()
                {
                    ProductId = p.ProductId,
                    CustomerId = customerId,
                };
                _context.RateProducts.Add(rateProduct);
            }
            await _context.SaveChangesAsync();
        }
        public int Delete(int ProductId, int CustomerId)
        {
            RateProduct rateProduct = _context.RateProducts.FirstOrDefault(x => x.ProductId == ProductId && x.CustomerId == CustomerId);
            if (rateProduct != null)
                _context.RateProducts.Remove(rateProduct);
            return _context.SaveChanges();
        }
    }
    public class RateProductTogether
    {
        public int ProductId { get; set; }
        public int PointCustomer1 { get; set; }
        public int PointCustomer2 { get; set; }
    }
    public class ProductSuggestion
    {
        public Product Product { get; set; }
        public double PredictRate { get; set; }
    }
    public class ItemMatrix
    {
        public int CustomerId { get; set; }
        public List<RateProduct> RateProducts { get; set; }
    }
}
