using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IgniteApp.Shell.Footer.ViewModels
{
    public class TTForgeViewModel : Screen
    {
        private int _number;

        public int Number
        {
            get => _number;
            set => SetAndNotify(ref _number, value);
        }

        private Timer timer;
        private DispatcherTimer dispatcherTimer;

        public TTForgeViewModel()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            // timer=new Timer();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Number++;
        }
    }
}