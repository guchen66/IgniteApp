using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Recipe.Models
{
    public class RecipeMenuItem : DaoViewModelBase, IMenuItem
    {
        public string MenuName { get; set; }
    }
}
