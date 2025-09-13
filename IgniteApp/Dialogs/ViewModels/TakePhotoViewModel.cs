using IgniteApp.Dialogs.Manage;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Attributes;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    [SingleNavigateScan("相机2")]
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