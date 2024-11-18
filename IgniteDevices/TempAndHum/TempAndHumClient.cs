using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.TempAndHum
{
    public class TempAndHumClient
    {
        public async Task<bool> Modify()
        {
            string portName = "COM4";
            int baudRate = 9600;
            Parity parity = Parity.None;
            int dataBIts = 8;
            StopBits stopBits = StopBits.One;

          

         /*   Task.Run(() =>
            {
              
            });
*/
            try
            {

                SerialPort serialPort = new SerialPort(portName);
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    await Task.Delay(1000);
                    SysTempAndHum.IsConnTemp = true;
                    SysTempAndHum.IsConnHum = true;
                    //serialPort.WriteLine("");
                    string dataReceived = serialPort.ReadLine();
                    SysTempAndHum.Temp = double.Parse(dataReceived);
                 /*   Task.Run(() =>
                    {
                        
                    });*/
                }
                 serialPort.Close();
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
