using AutoMapper;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Providers
{
    public class AutoMapperProvider : IAutoMapperProvider
    {
        private readonly MapperConfiguration _config;
        public AutoMapperProvider(IContainer container)
        {
            _config = new MapperConfiguration(configure => 
            {
                // 使用显式类型转换来调用正确的Get方法
                configure.ConstructServicesUsing(type => (object)container.Get(type));
                configure.AddMaps("IgniteAdmin");
            });
        }
        public IMapper GetMapper()
        {
            return _config.CreateMapper();
        }
    }
}
