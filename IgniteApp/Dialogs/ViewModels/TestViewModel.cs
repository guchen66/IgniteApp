using IgniteApp.Common;
using IgniteApp.Extensions;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Enums;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Events;
using IgniteApp.Shell.Footer.ViewModels;
using IT.Tangdao.Framework.Abstractions.Loggers;

namespace IgniteApp.Dialogs.ViewModels
{
    public class TestViewModel : Screen, IDialogProvider
    {
        private readonly IActionTable _handlerTable;
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(TestViewModel));

        public TestViewModel(IActionTable handlerTable)
        {
            _handlerTable = handlerTable;
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
            ActionResult result = new ActionResult();
            result.Result = ActionStatus.Success;
            // _handlerTable.GetResultHandler("Open").Invoke(result);
            _handlerTable.Execute("Open", result);
            Logger.WriteLocal($"TestViewModel测试：{_handlerTable.GetActionInfo().Count}");
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

        protected override void OnClose()
        {
            base.OnClose();

            _handlerTable.UnregisterResultHandler("Open");
        }
    }
}