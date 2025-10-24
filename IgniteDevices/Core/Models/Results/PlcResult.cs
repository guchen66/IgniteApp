using IT.Tangdao.Framework.Abstractions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Core.Models.Results
{
    // PLC结果（继承原有字段）
    public class PlcResult //: ResponseResult<PlcData>
    {
        public int ExecTimeMs { get; set; }
    }
}