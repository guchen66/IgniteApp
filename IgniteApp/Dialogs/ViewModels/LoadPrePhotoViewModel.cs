using IT.Tangdao.Framework.Abstractions.Navigation;
using IT.Tangdao.Framework.Attributes;
using Stylet;

namespace IgniteApp.Dialogs.ViewModels
{
    [SingleNavigateScan("相机")]
    public class LoadPrePhotoViewModel : Screen, ISingleNavigateView
    {
        public string ViewName => "上料预校";
        public int DisplayOrder => 1;
        public string GroupKey => "相机";

        public LoadPrePhotoViewModel()
        {
            DisplayName = ViewName;
        }
    }
}