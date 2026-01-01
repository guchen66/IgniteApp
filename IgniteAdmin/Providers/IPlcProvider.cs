using IgniteDevices.Connections;
using IT.Tangdao.Framework.Abstractions.Results;

namespace IgniteAdmin.Providers
{
    public interface IPlcProvider
    {
        ConnectionContext Context { get; }

        ResponseResult ConnectionSiglePLC();
    }
}