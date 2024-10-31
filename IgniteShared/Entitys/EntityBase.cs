using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 数据库实体类基类
    /// </summary>
    public class EntityBase
    {
        public long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}
