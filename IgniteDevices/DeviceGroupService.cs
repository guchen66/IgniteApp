using IgniteDevices.TempAndHum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices
{
    public class DeviceGroupService
    {
        private DeviceGroupService()
        {
                
        }
        public static DeviceGroupService Current => GetCurrent();

        private static DeviceGroupService GetCurrent()
        {
            return new DeviceGroupService();
        }

        public static void Modify()
        {

        }

        public static TempAndHumClient TempAndHum { get; set; }

    }
}
