using IgniteApp.Common;
using IgniteApp.Extensions;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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