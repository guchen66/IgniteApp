using IgniteApp.Common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Extensions
{
    /// <summary>
    /// 对IWindowManager进行扩展，增加打开弹窗可以传递参数
    /// </summary>
    public static class WindowManagerExtensions
    {
        public static DialogResult ShowDialogEx(this IWindowManager windowManager, object viewModel, DialogParameters parameters = null)
        {
            // 1. 注入输入参数
            if (viewModel is IDialogProvider dialogAware)
            {
                dialogAware.OnDialogOpened(parameters ?? new DialogParameters());
            }

            // 2. 显示对话框（原生 Stylet）
            bool? result = windowManager.ShowDialog(viewModel);

            // 3. 获取输出参数
            var dialogResult = new DialogResult { Result = result };
            if (viewModel is IDialogProvider dialogAwareOut)
            {
                var output = dialogAwareOut.OnDialogClosing();
                if (output != null)
                {
                    foreach (var kvp in output.OutputParameters)
                    {
                        dialogResult.AddOutput(kvp.Key, kvp.Value);
                    }
                }
            }

            return dialogResult;
        }
    }
}