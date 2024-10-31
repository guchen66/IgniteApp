using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 物料信息
    /// </summary>
    public class MaterialInfo:EntityBase
    {
        public string Station { get; set; }
        public string Status { get; set; }

    }
}
