using IgniteShared.Dtos;
using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.IRepositorys
{
    public interface IProductRepository
    {
        void AddProduct(ProductInfo productInfo);

        List<ProductDto> GetAllProductInfo();
    }
}
