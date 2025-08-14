using IgniteApp.Common;
using IgniteApp.Extensions;
using IgniteApp.Shell.Maintion.Services;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Dialogs.ViewModels
{
    public class TestViewModel : Screen, IDialogProvider
    {
        public TestViewModel()
        {
        }

        public DialogResult OnDialogClosing()
        {
            return new DialogResult()
            {
                Result = true,
                ResultValue = "Hello,World"
            };
        }

        private ObservableCollection<string> _propertyList;

        public ObservableCollection<string> PropertyList
        {
            get => _propertyList;
            set => SetAndNotify(ref _propertyList, value);
        }

        private BindableCollection<string> _strings;

        public BindableCollection<string> Strings
        {
            get => _strings;
            set => SetAndNotify(ref _strings, value);
        }

        public void OnDialogOpened(DialogParameters parameters)
        {
            Enumerable.Empty<string>();
        }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        private Dispatcher _dispatcher;

        public void Open()
        {
            // 获取当前是否全部连接（假设默认false）
            bool currentState = IsChecked;

            // 切换状态（true → 全部连接，false → 全部断开）
            bool newState = !currentState;
            IsChecked = newState;
            // 通过中介者通知所有设备
            DeviceMediator.Instance.NotifyAllDevices(newState);
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            var newData = PropertyList;
            _lock.EnterWriteLock();
            try
            {
                PropertyList.Clear();
                //PropertyList.Add(newData);
                List<string> lists = new List<string>();
                lists.AddRange(newData);
                Strings.AddRange(lists);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}