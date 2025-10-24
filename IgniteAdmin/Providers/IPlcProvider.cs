using IgniteShared.Models;
using IT.Tangdao.Framework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgniteDevices.Core.Models.Results;
using Modbus.Device;
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