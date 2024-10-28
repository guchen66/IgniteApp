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
        private string _station;

        public string Station
        {
            get => _station;
            set => SetProperty(ref _station, value);
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

    }
}
