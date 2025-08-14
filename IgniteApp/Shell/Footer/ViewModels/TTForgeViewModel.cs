using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

        private DispatcherTimer dispatcherTimer;
        private System.Timers.Timer _saveTimer;

        public TTForgeViewModel()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            // timer=new Timer();

            _saveTimer = new System.Timers.Timer(3000);  // 5秒保存一次
            _saveTimer.Elapsed += async (s, e) => await SaveReportToFileAsync();
            _saveTimer.AutoReset = true;
            _saveTimer.Start();

            // Loaded();
        }

        private void Loaded()
        {
            Task.Run(() =>
            {
                while (!manualResetEvent.WaitOne(500))
                {
                }
            });
        }

        public ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        private async Task SaveReportToFileAsync()
        {
            Number++;
            await Task.CompletedTask;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Number++;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }

        protected override void OnDeactivate()
        {
            _saveTimer?.Dispose();
            dispatcherTimer.Stop();
        }
    }

    public class LoadProcess
    {
        public void Execute()
        {
        }
    }

    public class PreProcess
    {
        public void Execute()
        { }
    }

    public class UVCutProcess
    {
        public void Execute()
        { }
    }

    public class CO2CutProcess
    {
        public void Execute()
        { }
    }

    public class LineScanProcess
    {
        public void Execute()
        { }
    }

    public class UnLoadProcess
    {
        public void Execute()
        { }
    }
}