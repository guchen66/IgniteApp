using IT.Tangdao.Framework.Mvvm;

namespace IgniteApp.Shell.Maintion.Args
{
    public class UsualArgs : ViewModelBase
    {
        private double[] _doubleValue;

        public double[] DoubleValue
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }
    }
}