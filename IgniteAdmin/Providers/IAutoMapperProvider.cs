using AutoMapper;

namespace IgniteAdmin.Providers
{
    public interface IAutoMapperProvider
    {
        IMapper GetMapper();
    }
}
