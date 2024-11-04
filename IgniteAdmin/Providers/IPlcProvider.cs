using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Providers
{
    public interface IPlcProvider
    {
        IPlcBuilder Builder();
        PlcBackResult ConnectionSiglePLC();
    }
}
