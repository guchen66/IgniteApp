using IT.Tangdao.Framework;
using IT.Tangdao.Framework.DaoAdmin.Navigates;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class UVTeachViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "";

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
        }
    }
}