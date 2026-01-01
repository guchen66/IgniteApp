using IgniteApp.Common;
using IgniteApp.Extensions;
using Stylet;

namespace IgniteApp.Dialogs.ViewModels
{
    public class SaveFormatViewModel : Screen, IDialogProvider
    {
        public DialogResult OnDialogClosing()
        {
            DialogResult dialogResult = new DialogResult();
            return dialogResult;
        }

        public void OnDialogOpened(DialogParameters parameters)
        {
        }
    }
}