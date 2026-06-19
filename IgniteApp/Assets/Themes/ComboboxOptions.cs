using IT.Tangdao.Framework.Markup;

namespace IgniteApp.Assets.Themes
{
    public class ComboboxOptions
    {
        public static void SetTheme()
        {
            OptionItemsSourceExtension.OptionsPool["Accuracy"] = new[] { "X1", "Y1", "X2", "Y2", };
        }
    }
}