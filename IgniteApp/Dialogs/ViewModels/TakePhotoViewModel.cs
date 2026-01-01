using IT.Tangdao.Framework.Abstractions.Navigation;
using IT.Tangdao.Framework.Attributes;
using Stylet;

namespace IgniteApp.Dialogs.ViewModels
{
    [SingleNavigateScan("相机")]
    public class TakePhotoViewModel : Screen, ISingleNavigateView
    {
        public string ViewName => "拍照前";
        public int DisplayOrder => 2;
        public string GroupKey => "相机2";

        public TakePhotoViewModel()
        {
            DisplayName = ViewName;
        }
    }
}