using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Managers.Home
{
    public class TimeManager
    {
        public string ShowCurrentTime()
        {
            return DateTime.Now.ToString(@"hh\:mm\:ss");
        }
    }
}
