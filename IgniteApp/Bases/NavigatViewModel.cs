using IgniteApp.Interfaces;
using IT.Tangdao.Framework.Abstractions.IServices;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Bases
{
    public abstract class NavigatViewModel : Conductor<IScreen>.Collection.OneActive
    {
        // public virtual string Name { get; set; }
        public IViewFactory ViewFactoryService;

        // private readonly IReadService Read;
        public readonly INavigateRoute NavigatRouteService;

        //public string HandlerName { get; set; }

        public NavigatViewModel()
        {
            NavigatRouteService = ServiceLocator.GetService<INavigateRoute>();
        }

        public void DoNavigateToView1(ref IScreen screen)
        {
            var s1 = screen.DisplayName;
            ActivateItem(ViewFactoryService.CreateViewModel(DisplayName));
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}