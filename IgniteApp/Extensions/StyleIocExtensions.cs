using Stylet;
using StyletIoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Extensions
{
    public static class StyleIocExtensions
    {
        public static Func<int> RegisterEvent;

        public static IBindTo Register(this IBindTo Bind, Func<int> action)
        {
            action.Invoke();
            return Bind;
        }
    }

    public static class WindowManagerExtensions
    {
        public static DialogResult ShowDialogEx(this IWindowManager windowManager, object viewModel, DialogParameters parameters = null)
        {
            // 1. 注入输入参数
            if (viewModel is IHaveDialogParameters dialogAware)
            {
                dialogAware.OnDialogOpened(parameters ?? new DialogParameters());
            }

            // 2. 显示对话框（原生 Stylet）
            bool? result = windowManager.ShowDialog(viewModel);

            // 3. 获取输出参数
            var dialogResult = new DialogResult { Result = result };
            if (viewModel is IHaveDialogParameters dialogAwareOut)
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

    // ViewModel 需实现的接口
    public interface IHaveDialogParameters
    {
        void OnDialogOpened(DialogParameters parameters);

        // 返回输出参数（返回 DialogResult）
        DialogResult OnDialogClosing();
    }

    public class DialogResult
    {
        private readonly Dictionary<string, object> _outputParameters = new Dictionary<string, object>();

        public bool? Result { get; set; }

        // 添加输出参数
        public DialogResult AddOutput(string key, object value)
        {
            _outputParameters[key] = value;
            return this;
        }

        // 获取输出参数
        public T GetOutput<T>(string key, T defaultValue = default)
        {
            return _outputParameters.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }

        // 直接访问字典（备用）
        public IReadOnlyDictionary<string, object> OutputParameters => _outputParameters;
    }

    public class DialogParameters
    {
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        // 添加参数（支持链式调用）
        public DialogParameters Add(string key, object value)
        {
            _parameters[key] = value;
            return this;
        }

        // 安全获取参数（泛型 + 默认值）
        public T GetValue<T>(string key, T defaultValue = default)
        {
            return _parameters.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }

        // 直接访问字典（备用）
        public IReadOnlyDictionary<string, object> Parameters => _parameters;
    }
}