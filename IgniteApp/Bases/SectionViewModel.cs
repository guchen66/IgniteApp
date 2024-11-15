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
}
