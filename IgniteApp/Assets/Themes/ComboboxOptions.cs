using IT.Tangdao.Framework.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Assets.Themes
{
    public class ComboboxOptions
    {
        public static void SetTheme()
        {
            OptionListExtension.OptionsPool["Accuracy"] = new[] { "X1", "Y1", "X2", "Y2", "X3", "Y3" };
        }
    }
}