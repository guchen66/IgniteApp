using System;

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
