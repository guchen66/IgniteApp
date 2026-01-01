namespace IgniteShared.Models
{
    public class PlcConfig
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string PlcType { get; set; }
        public string ComPort { get; set; }
        public int BaudRate { get; set; }

        // public ConnectionType ConnectionType { get; set; }
    }
}