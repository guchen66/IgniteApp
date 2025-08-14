using IgniteApp.Dialogs.Manage;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    public class LoadPrePhotoViewModel : Screen, IPhotoView
    {
        public string ViewName => "上料预校";
        public int DisplayOrder => 1;

        // 业务逻辑...
    }
}