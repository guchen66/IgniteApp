using IgniteApp.Dialogs.Manage;
using IgniteApp.Interfaces;
using IgniteApp.Tests;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using IT.Tangdao.Framework.DaoAttributes;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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