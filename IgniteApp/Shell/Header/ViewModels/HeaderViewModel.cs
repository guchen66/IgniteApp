using IgniteApp.ViewModels;
using IT.Tangdao.Framework.DaoCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using IgniteApp.Bases;

namespace IgniteApp.Shell.Header.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        #region--属性--
        private Window CurrentWindow => GetCurrentWindow();

        #endregion

        #region--ctor
        public HeaderViewModel()
        {
            DragmoveCommand = MinidaoCommand.Create(ExecuteDragmove);
            MinCommand = MinidaoCommand.Create(ExecuteMin);
            MaxCommand = MinidaoCommand.Create(ExecuteMax);
            CloseCommand = MinidaoCommand.Create(ExecuteClose);
        }

        #endregion

        #region--命令--

        public ICommand DragmoveCommand { get; set; }
        public ICommand MinCommand { get; set; }
        public ICommand MaxCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        #region--方法--

        private Window GetCurrentWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
        }

        private void ExecuteDragmove()
        {

            CurrentWindow.DragMove();
        }

        private void ExecuteClose()
        {
            Application.Current.Shutdown();
        }

        private void ExecuteMax()
        {
            WindowState state = CurrentWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            CurrentWindow.WindowState = state;
        }

        private void ExecuteMin()
        {
            if (CurrentWindow.WindowState == WindowState.Maximized || CurrentWindow.WindowState == WindowState.Normal)
            {
                CurrentWindow.WindowState = WindowState.Minimized;
            }
        }

        #endregion
    }

    
}
