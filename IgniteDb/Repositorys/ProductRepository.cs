using IgniteDb.IRepositorys;
using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly AccessDbContext _context;
        public ProductRepository(AccessDbContext context)
        {
            _context = context;
        }
        public void AddProduct(ProductInfo productInfo)
        {
            _context.Products.Add(productInfo);
            _context.SaveChanges();
        }
    }
}
