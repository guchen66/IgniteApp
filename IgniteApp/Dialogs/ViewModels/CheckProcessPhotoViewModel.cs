using IT.Tangdao.Framework.Abstractions.Navigation;
using IT.Tangdao.Framework.Attributes;
using Stylet;

namespace IgniteApp.Dialogs.ViewModels
{
    [SingleNavigateScan("相机")]
    public class CheckProcessPhotoViewModel : Screen, ISingleNavigateView
    {
        public string ViewName => "检测流程";
        public int DisplayOrder => 3;

        public string GroupKey => "相机";

        public CheckProcessPhotoViewModel()
        {
            DisplayName = ViewName;
        }
    }
}