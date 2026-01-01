using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigation;
using Stylet;
using StyletIoC;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class TeachingViewModel : Screen
    {
        public ITangdaoRouter Router { get; set; }

        public TeachingViewModel(ITangdaoRouter router)
        {
            Router = router;
            Router.RegisterPage<CO2TeachViewModel>();
            Router.RegisterPage<UVTeachViewModel>();
            Router.RegisterPage<IRTeachViewModel>();
        }

        public void OpenTeachView(string navigateName)
        {
            TangdaoParameter tangdaoParameter = new TangdaoParameter();
            tangdaoParameter.Add("Name", "张三");
            Router.NavigateTo(navigateName, tangdaoParameter);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Router.LoadFirstPage();
        }
    }
}