using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Dtos
{
    public class ProductDto : DataBase
    {
        private string _productName;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

    }
}
