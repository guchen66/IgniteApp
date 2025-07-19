using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Providers
{
    public interface IAutoRun
    {
        Task Run();
        Task Stop();
    }
}
