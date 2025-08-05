using IgniteDevices.Core.Models.Results;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Connections
{
    public class ConnectionResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ConnectionResult(bool isSucess, string message)
        {
            IsSuccess = isSucess;
            Message = message;
        }
    }
}