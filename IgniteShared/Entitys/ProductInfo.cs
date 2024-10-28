using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductInfo: EntityBase
    {
        private string _productName;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

    }
}
