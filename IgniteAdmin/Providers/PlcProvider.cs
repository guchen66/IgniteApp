using AutoMapper;
using IgniteDevices;
using IgniteDevices.PLC;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoDtos.Options;
using IT.Tangdao.Framework.DaoEnums;
using IT.Tangdao.Framework.DaoIoc;
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
        public PlcClient Client { get; set; }
        public IPlcBuilder Builder()
        {
            PlcIocService.RegisterPlcServer(new PlcOption()
            {
                IsAutoConnection = true,
                PlcIpAddress = "127.0.0.1",
                Port="502",
                PlcType = PlcType.Siemens

            });
           
            throw new NotImplementedException();
        }


        private readonly MapperConfiguration _config;
        public PlcProvider(IContainer container)
        {
            _config = new MapperConfiguration(configure =>
            {
                // 使用显式类型转换来调用正确的Get方法
                configure.ConstructServicesUsing(type => (object)container.Get(type));
                configure.AddMaps("IgniteAdmin");
            });
        }
        public PlcBackResult ConnectionSiglePLC()
        {
            try
            {
                PlcResult = new PlcBackResult();
                TcpClient tcpClient = new TcpClientWithTimeout("127.0.0.1", int.Parse("502"), 1000).Connect();      //超时时间1秒
                if (!tcpClient.Connected)
                {
                    PlcResult.Message = "连接失败.";
                    PlcResult.IsSuccess = false;
                    return PlcResult;
                }

                // 创建Modbus连接
              
                PlcResult.IsSuccess = true;
                PlcResult.Message = "连接成功.";
            }
            catch (Exception ex)
            {
                PlcResult.Message = $"连接失败.{ex.Message}";
                PlcResult.IsSuccess = false;
            }
            return PlcResult;
        }
        public PlcBackResult PlcResult { get; set; }   //PLC的返回结果
    }
}
