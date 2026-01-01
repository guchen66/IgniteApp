using IgniteDevices.TempAndHum;

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
