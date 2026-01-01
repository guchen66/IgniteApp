using IgniteApp.Bases;
using IgniteApp.Shell.Set.Models;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class NetSetViewModel : ViewModelBase
    {
        private NetSetItem _netSetItem;

        public NetSetItem NetSetItem
        {
            get => _netSetItem;
            set => SetAndNotify(ref _netSetItem, value);
        }

    }
}
