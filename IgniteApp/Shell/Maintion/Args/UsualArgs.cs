using IT.Tangdao.Framework.Mvvm;

namespace IgniteApp.Shell.Maintion.Args
{
    public class UsualArgs : DaoViewModelBase
    {
        private double[] _doubleValue;

        public double[] DoubleValue
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }
    }
}