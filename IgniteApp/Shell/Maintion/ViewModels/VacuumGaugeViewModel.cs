using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigation;
using Stylet;

namespace IgniteApp.Shell.Maintion.ViewModels
{
    public class VacuumGaugeViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "真空压力表2222";

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            DisplayName = "真空压力表";
        }
    }
}