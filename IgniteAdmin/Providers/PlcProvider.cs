using AutoMapper;
using IgniteDevices;
using IgniteDevices.Connections;
using IgniteDevices.Core.Models.Results;
using IgniteDevices.PLC;
using IgniteDevices.PLC.Services;
using IgniteShared.Extensions;
using IgniteShared.Globals.System;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoDtos.Options;
using IT.Tangdao.Framework.DaoEnums;
using IT.Tangdao.Framework.DaoIoc;
using IT.Tangdao.Framework.Extensions;
using Modbus.Device;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Providers
{
    public class PlcProvider : IPlcProvider
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(PlcProvider));
        private ConnectionContext _context;
        public ConnectionContext Context => _context;

        public PlcProvider(ConnectionContext context)
        {
            _context = context;
        }

        public PlcResult ConnectionSiglePLC()
        {
            var plcResult = new PlcResult();
            try
            {
                Logger.WriteLocal("开始PLC连接流程");
                var result = Context.Connect();

                plcResult.IsSuccess = result.IsSuccess;
                plcResult.Message = result.Message;

                Logger.WriteLocal(result.IsSuccess
                    ? "PLC连接成功"
                    : $"PLC连接失败: {result.Message}");
            }
            catch (Exception ex)
            {
                plcResult.IsSuccess = false;
                plcResult.Message = $"连接异常: {ex.Message}";
                Logger.WriteLocal($"PLC连接异常: {ex.Message}");
            }

            return plcResult;
        }
    }
}