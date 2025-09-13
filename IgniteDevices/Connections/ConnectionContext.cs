using IgniteDevices.Connections.Interfaces;
using IgniteDevices.PLC.Services;
using IgniteShared.Enums;
using IgniteShared.Extensions;
using IgniteShared.Models;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Extensions;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteDevices.Connections
{
    public class ConnectionContext : IDisposable
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(ConnectionContext));
        private readonly List<IConnectionState> _connectionStates;
        private const int MaxRetriesPerState = 5;  //最大重试次数
        private int _currentRetryCount = 0;  //重试次数
        private int _currentStateIndex = 0;
        private IModbusMaster _master;
        private readonly object _lock = new object();
        public IReadOnlyList<IConnectionState> ConnectionStates => _connectionStates.AsReadOnly();

        /// <summary>
        /// 成功连接标志，用于触发重连
        /// </summary>
        public bool _isConnected;

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                ConnectionStateChanged.Invoke(this, new ConnectionStateEventArgs(value));
                // OnConnectionStateChanged(ConnectionResult.IsSuccess, ConnectionResult.Message);
            }
        }

        //  public IModbusMaster Master => _master ?? throw new InvalidOperationException("未建立连接");
        private IPlcConfigService _plcConfigService;

        public IModbusMaster Master
        {
            get
            {
                lock (_lock)
                {
                    return _master ?? throw new InvalidOperationException("未建立连接");
                }
            }
        }

        public ConnectionContext(IPlcConfigService plcConfigService)
        {
            _plcConfigService = plcConfigService;
            var config = _plcConfigService.GetConfig();
            _connectionStates = new List<IConnectionState>
            {
                new TcpState(config.IP, config.Port.ToInt()),     // 优先TCP连接
                new SerialState(config.ComPort, config.BaudRate)  // 其次串口连接
            };
        }

        public ConnectionResult ConnectionResult { get; private set; }

        public ConnectionResult Connect()
        {
            lock (_lock)
            {
                while (_currentStateIndex < _connectionStates.Count)
                {
                    var currentState = _connectionStates[_currentStateIndex];
                    ConnectionResult = currentState.Connect();
                    _currentRetryCount++;

                    Logger.WriteLocal($"{currentState.Type} ：尝试第 {_currentRetryCount}次连接， {(ConnectionResult.IsSuccess ? "成功" : "失败")}");

                    if (ConnectionResult.IsSuccess)
                    {
                        _master = _connectionStates[_currentStateIndex].CreateModbusMaster();
                        //  IsConnected = true;
                        return ConnectionResult;
                    }

                    if (_currentRetryCount >= MaxRetriesPerState)
                    {
                        Logger.WriteLocal($"{currentState.Type} 连接已达到最大重试次数");
                        _currentStateIndex++;  // 模拟状态模式切换到下一种连接方式
                        _currentRetryCount = 0;//重置次数
                    }
                }
                //  IsConnected = false;
                return new ConnectionResult(false, "所有连接方式均失败");
            }
        }

        public event EventHandler<ConnectionStateEventArgs> ConnectionStateChanged;

        private void OnConnectionStateChanged(bool isConnected, string message = null)
        {
            ConnectionStateChanged?.Invoke(this,
                new ConnectionStateEventArgs(isConnected, message));
        }

        public void Dispose()
        {
            // 清理资源
            foreach (var state in _connectionStates)
            {
                if (state is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }

    public class ConnectionStateEventArgs : EventArgs
    {
        public bool IsConnected { get; }
        public string Message { get; }

        public ConnectionStateEventArgs(bool isConnected, string message = null)
        {
            IsConnected = isConnected;
            Message = message;
        }
    }
}