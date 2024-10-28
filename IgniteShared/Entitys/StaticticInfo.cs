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
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Ok数量
        /// </summary>
        private int _OkCount;

        public int OkCount
        {
            get => _OkCount;
            set => SetProperty(ref _OkCount, value);
        }

        /// <summary>
        /// Ng数量
        /// </summary>
        private int _NgCount;

        public int NgCount
        {
            get => _NgCount;
            set => SetProperty(ref _NgCount, value);
        }

        /// <summary>
        /// 当日产量
        /// </summary>
        private int _outputCount;

        public int OutputCount
        {
            get => _outputCount;
            set => SetProperty(ref _outputCount, value);
        }

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public void SetReset()
        {
            this.OkCount = 0;
            this.NgCount = 0;
            this.OutputCount = 0;
        }
    }
}
