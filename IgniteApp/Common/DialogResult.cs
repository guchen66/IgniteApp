using IgniteDevices.Core.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
    public class DialogResult
    {
        private readonly Dictionary<string, object> _outputParameters = new Dictionary<string, object>();

        public bool? Result { get; set; }

        public object ResultValue { get; set; }

        public static DialogResult None => new DialogResult() { Result = false };

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
}