using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Events
{
    public class CloseRegisterEvent
    {
        public string Name { get; set; }
        public string Pwd { get; set; }

        public CloseRegisterEvent(string name, string pwd)
        {
            Name = name;
            Pwd = pwd;
        }
    }
}