using IgniteShared.Dtos;
using IgniteShared.Entitys;
using System.Collections.Generic;

namespace IgniteDb.IRepositorys
{
    public interface IProductRepository
    {
        void AddProduct(ProductInfo productInfo);

        List<ProductDto> GetAllProductInfo();
    }
}
