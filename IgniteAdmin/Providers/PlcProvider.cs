using IgniteDevices.Connections;
using IgniteDevices.Core.Models.Results;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Results;
using IT.Tangdao.Framework.Extensions;
using System;

namespace IgniteAdmin.Providers
{
    public class PlcProvider : IPlcProvider
    {
        private static readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(PlcProvider));
        private ConnectionContext _context;
        public ConnectionContext Context => _context;

        public PlcProvider(ConnectionContext context)
        {
            _context = context;
        }

        public ResponseResult ConnectionSiglePLC()
        {
            var plcResult = new PlcResult();
            try
            {
                Logger.WriteLocal("开始PLC连接流程");
                var result = Context.Connect();

                Logger.WriteLocal(result.IsSuccess
                    ? "PLC连接成功"
                    : $"PLC连接失败: {result.Message}");
                return ResponseResult.Success(value: result.Message);
            }
            catch (Exception ex)
            {
                Logger.WriteLocal($"PLC连接异常: {ex.Message}");

                return ResponseResult.FromException(ex, "连接异常");
            }
        }
    }
}