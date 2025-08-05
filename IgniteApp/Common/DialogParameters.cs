using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
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