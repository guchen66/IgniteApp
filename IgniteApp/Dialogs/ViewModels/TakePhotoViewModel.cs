using IgniteApp.Dialogs.Manage;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    public class TakePhotoViewModel : Screen, IPhotoView
    {
        public string ViewName => "拍照前";
        public int DisplayOrder => 2;

        // 业务逻辑...
    }
}