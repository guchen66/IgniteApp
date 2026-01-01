using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Navigation;
using Stylet;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class DigitalSmartGaugeViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "数字智能测量仪222";
        private IContentAccess _contentAccess;

        public DigitalSmartGaugeViewModel(IContentAccess contentAccess)
        {
            _contentAccess = contentAccess;
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            DisplayName = "数字智能测量仪";
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}