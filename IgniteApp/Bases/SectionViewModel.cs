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
        public virtual string Name { get; set; }
        
    }

    public class ChildrenClass : IConductor<string>
    {
        public bool DisposeChildren { get; set ; }

        public void ActivateItem(string item)
        {
            throw new NotImplementedException();
        }

        public void CloseItem(string item)
        {
            throw new NotImplementedException();
        }

        public void DeactivateItem(string item)
        {
            throw new NotImplementedException();
        }
    }
}
