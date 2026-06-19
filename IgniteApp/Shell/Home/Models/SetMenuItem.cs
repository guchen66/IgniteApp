using IT.Tangdao.Framework.Mvvm;

namespace IgniteApp.Shell.Home.Models
{
    public class SetMenuItem : ViewModelBase
    {
        public int Id { get; set; }
        private string _setMenuName;

        public string SetMenuName
        {
            get => _setMenuName;
            set => SetProperty(ref _setMenuName, value);
        }

        private string _setMenuToView;

        public string SetMenuToView
        {
            get => _setMenuToView;
            set => SetProperty(ref _setMenuToView, value);
        }

        public string MenuName { get; set; }
    }
}