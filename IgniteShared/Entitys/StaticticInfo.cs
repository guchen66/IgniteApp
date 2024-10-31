using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 统计信息
    /// </summary>
    public class StaticticInfo:EntityBase
    {
        public string Name { get; set; }

        /// <summary>
        /// Ok数量
        /// </summary>
        public int OkCount { get; set; }

        /// <summary>
        /// Ng数量
        /// </summary>
        public int NgCount { get; set; }

        /// <summary>
        /// 当日产量
        /// </summary>
        public int OutputCount { get; set; }
    }
}
