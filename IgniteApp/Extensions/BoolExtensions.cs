using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Extensions
{
    public static class BoolExtensions
    {
        public static string ToYAndN(this bool value)
        {
            return value ? "Y" : "N";
        }
    }
}