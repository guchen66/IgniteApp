using IgniteApp.Common;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace IgniteApp.Shell.Home.Models
{
    public struct HomeMenuItem
    {
        public string Title { get; set; }

        public string ViewModelName { get; set; }

        public IScreen ViewModel { get; set; }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetItem()
        {
            this = new HomeMenuItem();
        }
    }
}