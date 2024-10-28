using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    /// <summary>
    /// 由于没有使用数据库
    /// 这里直接使用实体类在界面展示
    /// </summary>
    public class EntityBase:DaoViewModelBase
    {
        private long _id;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _createTime;

        public DateTime CreateTime
        {
            get => _createTime;
            set => SetProperty(ref _createTime, value);
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime _updateTime;

        public DateTime UpdateTime
        {
            get => _updateTime;
            set => SetProperty(ref _updateTime, value);
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _remark;

        public string Remark
        {
            get => _remark;
            set => SetProperty(ref _remark, value);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        private int _delete;

        public int Delete
        {
            get => _delete;
            set => SetProperty(ref _delete, value);
        }

    }
}
