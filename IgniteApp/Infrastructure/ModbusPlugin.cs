using IT.Tangdao.Framework.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Infrastructure
{
    public sealed class PluginCaller
    {
        private readonly ITangdaoMessage _plugin;

        public PluginCaller(ITangdaoMessage plugin)
        {
            _plugin = plugin;
        }

        /// <summary>
        /// 强类型入口：await 就能拿到结果
        /// </summary>
        public Task<ushort> ReadRegAsync(byte address)
        {
            var request = new TangdaoRequest();
            request.Address = address;

            // 一击即中
            _plugin.Response(request);

            // 等插件 SetResult
            return request.ReplySource.Task.ContinueWith(t => (ushort)t.Result);
        }
    }
}