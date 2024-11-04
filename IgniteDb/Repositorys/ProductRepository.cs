using AutoMapper;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
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
        private IMapper _mapper;
        public ProductRepository(AccessDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddProduct(ProductInfo productInfo)
        {
            _context.Products.Add(productInfo);
            _context.SaveChanges();
        }

        public List<ProductDto> GetAllProductInfo()
        {
           var productInfos= _context.Products.ToList();
            List<ProductDto> dtos = _mapper.Map<List<ProductDto>>(productInfos);
            return dtos;
        }
    }
}
