using IT.Tangdao.Framework.Mvvm;

namespace IgniteApp.Shell.Set.Models
{
    public class AccountSetItem : ViewModelBase
    {
        private string _account;

        public string Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

    }
}
