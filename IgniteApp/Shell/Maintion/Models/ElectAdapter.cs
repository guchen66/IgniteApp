using IgniteApp.Shell.Maintion.Args;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Models
{
    /// <summary>
    /// 适配器模式
    /// 当状态改变不能跟据某个属性改变的时候，自己去创造对象
    /// </summary>
    public class ElectAdapter : INotifyPropertyChanged, IDisposable
    {
        private readonly IElectService _elect;
        private readonly ElectricityModel _model;

        public ElectAdapter(IElectService elect, ElectricityModel model)
        {
            _elect = elect;
            _model = model;

            _elect.StatusChanged += OnStatusChanged; // 订阅公司接口的事件
        }

        private void OnStatusChanged()
        {
            // 通过模型映射，触发带参数的内部事件
            StatusChanged?.Invoke(_model);
        }

        public event Action<ElectricityModel> StatusChanged; // 自定义带模型参数的事件

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            _elect.StatusChanged -= OnStatusChanged; // 防止内存泄漏
        }
    }
}