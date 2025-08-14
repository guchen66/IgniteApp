using IgniteApp.Common;
using IgniteApp.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IgniteApp.Interfaces
{
    public interface IDialogService
    {
        void Show(object viewModel, DialogParameters parameters = null, Action<DialogResult> result = null);

        Task<DialogResult> ShowAsync(object viewModel, DialogParameters parameters = null);
    }

    public class DialogService : IDialogService
    {
        public async Task<DialogResult> ShowAsync(object viewModel, DialogParameters parameters = null)
        {
            if (viewModel is IDialogProvider provider)
            {
                provider.OnDialogOpened(parameters ?? new DialogParameters());
            }

            var win = ViewLocator.Build1(viewModel);
            await Task.Delay(500);
            // 使用ShowDialog而不是Show以实现模态对话框
            win.ShowDialog();
            win.Closed += (s, e) =>
            {
                if (viewModel is IDialogProvider dialogAwareClosed)
                {
                    //dialogAwareClosed.OnDialogClosed();
                }
            };
            return DialogResult.None;
        }

        // 保留原有同步方法
        public void Show(object viewModel, DialogParameters parameters = null, Action<DialogResult> result = null)
        {
            var dialogResult = ShowAsync(viewModel, parameters).GetAwaiter().GetResult();
            result?.Invoke(dialogResult);
        }

        public class ViewLocator
        {
            public static IDialogWindow Build(object data)
            {
                if (data is null)
                    return null;

                var name = data.GetType().FullName.Replace("ViewModel", "View");
                var type = Type.GetType(name);

                if (type != null)
                {
                    var control = (DialogWindow)Activator.CreateInstance(type);
                    control.DataContext = data;
                    return control;
                }

                return new DialogWindow();
            }

            public static Window Build1(object data)
            {
                if (data is null)
                    return null;

                var name = data.GetType().FullName.Replace("ViewModel", "View");
                var type = Type.GetType(name);

                if (type != null)
                {
                    var control = (Window)Activator.CreateInstance(type);
                    control.DataContext = data;
                    return control;
                }

                return new Window();
            }
        }

        public interface IDialogWindow
        {
            void Show(Action<DialogResult> result);
        }

        public class DialogWindow : Window, IDialogWindow
        {
            void IDialogWindow.Show(Action<DialogResult> result)
            {
                Show();
            }
        }
    }
}