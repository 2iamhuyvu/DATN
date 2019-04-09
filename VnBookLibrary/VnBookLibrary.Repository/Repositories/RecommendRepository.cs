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
    public class RecommendRepository : GenericRepository<Recommend>
    {
        public RecommendRepository(VnBookLibraryDbContext context) : base(context)
        {
        }
        public RecommendRepository() : base()
        {
        }
        public async Task InsertOrUpdateAsync(Recommend recommend)
        {
            recommend.Count = 1;
            var r = _context.Recommends.FirstOrDefault(x => ((x.ProductId1 == recommend.ProductId1 && x.ProductId2 == recommend.ProductId2) || (x.ProductId1 == recommend.ProductId2 && x.ProductId2 == recommend.ProductId1)));
            if (r != null)
            {
                r.Count++;
            }
            else
            {
                _context.Recommends.Add(recommend);
            }
            await _context.SaveChangesAsync();
        }
        public async Task InsertOrUpdateAsync(List<BillDetail> billDetails)
        {
            if (billDetails.Count >= 2)
            {
                for (int i = 0; i < billDetails.Count - 1; i++)
                {
                    for (int j = i + 1; j < billDetails.Count; j++)
                    {
                        Recommend recommend = new Recommend()
                        {
                            ProductId1 = billDetails[i].ProductId,
                            ProductId2 = billDetails[j].ProductId,
                        };
                        await InsertOrUpdateAsync(recommend);
                    }
                }
            }
        }
        private List<Recommend> GetRecommendByProduct(int productId)
        {
            var p = _context.Products.Find(productId);
            List<Recommend> rs = new List<Recommend>();
            var recommends = _context.Recommends.Where(x => x.ProductId1 == productId || x.ProductId2 == productId).OrderByDescending(x => x.Count).ToList();
            if (recommends != null && recommends.Count > 0)
            {
                foreach (var item in recommends)
                {
                    Recommend r = new Recommend();
                    r.ProductId1 = productId;
                    r.RecommendId = item.RecommendId;
                    r.Product1 = p;
                    r.Count = item.Count;
                    if (item.ProductId2 == productId)
                    {
                        r.ProductId2 = item.ProductId1;
                        r.Product2 = item.Product1;
                    }
                    else
                    {
                        r.ProductId2 = item.ProductId2;
                        r.Product2 = item.Product2;
                    }
                    rs.Add(r);
                }
            }
            return rs;
        }
        public List<Product> GetRecommendProductByListProduct(List<Product> products)
        {
            List<Product> result = new List<Product>();
            List<Recommend> rs = new List<Recommend>();
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    List<Recommend> recommends = GetRecommendByProduct(product.ProductId);
                    rs.AddRange(recommends);
                }
            }
            if (rs != null && rs.Count > 0)
            {
                rs = rs.OrderByDescending(x => x.Count).ToList();
                List<Recommend> temp = new List<Recommend>();
                for (int i = 0; i < rs.Count; i++)
                {
                    if (!(CheckExist1(temp, rs[i].ProductId2 ?? 0) || CheckExist2(products, rs[i].ProductId2 ?? 0)))
                    {
                        temp.Add(rs[i]);
                        result.Add(rs[i].Product2);
                    }
                }
            }
            return result;
        }

        private bool CheckExist1(List<Recommend> recommends, int productid)
        {
            if (recommends != null && recommends.Count > 0)
            {
                foreach (var item in recommends)
                {
                    if (item.ProductId2 == productid)
                        return true;
                }
            }
            return false;
        }
        private bool CheckExist2(List<Product> products, int productid)
        {
            if (products != null && products.Count > 0)
            {
                foreach (var item in products)
                {
                    if (item.ProductId == productid)
                        return true;
                }
            }
            return false;
        }
        public List<Product> GetRecommendProductByProduct(int productId)
        {
            List<Product> rs = new List<Product>();
            var r = GetRecommendByProduct(productId);
            if (r != null & r.Count > 0)
            {
                foreach (var item in r)
                {
                    rs.Add(item.Product2);
                }
            }
            return rs;
        }
        private List<Product> GetPurchasedProducts(int customerId)
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
        public List<Product> GetRecommendProductByCustomer(int customerId)
        {
            List<Product> products = GetPurchasedProducts(customerId);
            return GetRecommendProductByListProduct(products);
        }
    }
}
