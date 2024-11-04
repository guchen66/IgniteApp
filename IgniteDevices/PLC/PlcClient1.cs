using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteDevices.PLC
{
    public class PlcClient1
    {
        /// <summary>
        /// 当存在多个PLC实例时
        /// </summary>
        /// <param name="plcId"></param>
        /// <returns></returns>
        public IModbusMaster GetClient(int plcId)
        {
            if (_modbusMasters.ContainsKey(plcId))
            {
                return _modbusMasters[plcId];
            }

            lock (_lockObject)
            {
                if (!_modbusMasters.ContainsKey(plcId))
                {
                    var plcInfo = FindRuningPlc.FirstOrDefault(p => p.Id == plcId);
                    if (plcInfo != null)
                    {
                        TcpClient tcpClient = new TcpClientWithTimeout(plcInfo.IP, int.Parse(plcInfo.Port), 1000).Connect();
                        //   TcpClient tcpClient = new TcpClient();
                        tcpClient.Connect(plcInfo.IP, int.Parse(plcInfo.Port)); // 假设Port也从配置中获取
                        if (tcpClient.Connected)
                        {
                            var modbusMaster = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);
                            _modbusMasters[plcId] = modbusMaster;
                            return modbusMaster;
                        }
                    }
                }
            }
            return null;
        }
        public int SavleId { get; set; }           //PLC从站ID
        private readonly object _lockObject = new object();
        public PlcInfo[] FindRuningPlc { get; set; }
        public IModbusMaster CreateClient => GetClient(SavleId);         //多个PLC
        private readonly Dictionary<int, IModbusMaster> _modbusMasters = new Dictionary<int, IModbusMaster>();
    }
    /// <summary>
    /// PLC的基础信息
    /// 如果View以实例去绑定，那么构造器进行静态的赋值
    /// </summary>
    public class PlcInfo 
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string PlcType { get; set; }

        public string PlcName { get; set; }
    }

   
}
