using IgniteApp.ViewModels;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Dialogs.ViewModels
{
    public class SetCalibrationParameterViewModel : Screen, IGuardClose
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public event Action<string> OnSaved;

        public async void Save()
        {
            // 检查是否可以关闭
            bool canClose = await CanCloseAsync();
            try
            {
                if (canClose)
                {
                    // 标记数据已保存（避免二次提示）
                    IsDataSaved = true;

                    // 真正关闭窗口（带 DialogResult = true）
                    RequestClose(true);
                }
                else
                {
                    // 不能关闭（例如用户点了“取消”）
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsDataSaved = false;
            }
        }

        public bool IsDataSaved { get; set; }

        public override async Task<bool> CanCloseAsync()
        {
            if (!IsDataSaved)
            {
                // 异步模拟（比如网络请求）
                await Task.Delay(1000);

                // 弹窗询问用户
                var result = MessageBox.Show("数据未保存，确定关闭吗？", "警告", MessageBoxButton.YesNo);
                return result == MessageBoxResult.Yes;
            }
            else
            {
                // 数据已保存，允许关闭
                return true;
            }
        }
    }
}