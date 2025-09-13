using IgniteApp.Common;
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