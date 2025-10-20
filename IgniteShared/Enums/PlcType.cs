using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Enums
{
    public enum PlcType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 西门子
        /// </summary>
        Siemens = 1,

        /// <summary>
        /// 三菱
        /// </summary>
        Mitsubishi = 2,

        /// <summary>
        /// 欧姆龙
        /// </summary>
        Omron = 3,

        /// <summary>
        /// 施耐德
        /// </summary>
        Schineider = 4,
    }
}