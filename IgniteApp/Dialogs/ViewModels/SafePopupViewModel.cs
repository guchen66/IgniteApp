using IgniteApp.Bases;
using IgniteShared.Globals.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    public class SafePopupViewModel: ViewModelBase
    {
        public bool IsShould;

        private string _password;

        public string Password
        {
            get => _password;
            set => SetAndNotify(ref _password, value);
        }

        public void ExecuteClose()
        {
            if (Password== SysLoginInfo.Password)
            {
                this.RequestClose(true);
            }
           else 
            {
                this.RequestClose(false);
            }
        }
    }
}
