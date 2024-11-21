using IgniteApp.Interfaces;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Bases
{
    public class NavigatViewModel : Conductor<IScreen>.Collection.OneActive
    {
       // public virtual string Name { get; set; }
        public IViewFactory ViewFactoryService;
        public readonly INavigateRoute NavigatRouteService;
        public NavigatViewModel()
        {
            NavigatRouteService = ServiceLocator.GetService<INavigateRoute>();
        }

        public void DoNavigateToView1(ref IScreen screen)
        {
           
            var s1=screen.DisplayName;
            ActivateItem(ViewFactoryService.CreateViewModel(DisplayName));
        }
    }

}
