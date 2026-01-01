using System;

namespace IgniteAdmin.Managers.Home
{
    public class TimeManager
    {
        public string ShowCurrentTime()
        {
            string sss = "";

            return DateTime.Now.ToString(@"hh\:mm\:ss");
        }
    }
}