using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Maintion.Models
{
    public class ElectStatus : DaoViewModelBase
    {
        private ElectPriceTrace electPriceTrace;

        public ElectStatus()
        {
            electPriceTrace = new ElectPriceTrace();
            electPriceTrace.PriceChanged += ElectPriceTrace_PriceChanged;
        }

        private void ElectPriceTrace_PriceChanged(object sender, ElectPriceArgs e)
        {
        }

        private double _electPrice;

        public double ElectPrice
        {
            get => _electPrice;
            set
            {
                _electPrice = value;
                RaisePropertyChanged(nameof(ElectPrice));
            }
        }

        private string _status;

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
    }

    public class ElectPriceArgs : EventArgs
    {
        public string Symbol { get; }
        public decimal NewPrice { get; }

        public ElectPriceArgs(string symbol, decimal newPrice)
        {
            Symbol = symbol;
            NewPrice = newPrice;
        }
    }

    public class ElectPriceTrace
    {
        public event EventHandler<ElectPriceArgs> PriceChanged;

        public void UpdatePrice(string symbol, decimal newPrice)
        {
            // 无论涨跌都触发同一事件
            PriceChanged?.Invoke(this, new ElectPriceArgs(symbol, newPrice));
        }
    }
}