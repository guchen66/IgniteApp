using IgniteApp.Dialogs.Manage;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    public class CheckProcessPhotoViewModel : Screen, IPhotoView
    {
        public string ViewName => "检测流程";
        public int DisplayOrder => 3;

        // 业务逻辑...
    }
}