using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgniteDevices.Core.Models.Results;
using Modbus.Device;
using IgniteDevices.Connections;

namespace IgniteAdmin.Providers
{
    public interface IPlcProvider
    {
        ConnectionContext Context { get; }

        PlcResult ConnectionSiglePLC();
    }
}