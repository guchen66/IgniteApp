﻿using IgniteApp.Interfaces;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Modules
{
    public class HomeModules : StyletIoCModule
    {
        protected override void Load()
        {
            Bind<IViewFactory>().ToAbstractFactory();
        }
    }
}
