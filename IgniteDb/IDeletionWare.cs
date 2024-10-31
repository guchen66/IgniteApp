using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb
{
    /// <summary>
    /// 软删除实现的接口
    /// </summary>
    public interface IDeletionWare
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeleteTime { get; set; }
    }
}
