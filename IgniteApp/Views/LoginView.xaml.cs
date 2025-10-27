using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IgniteApp.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }
    }

    public class DragMoveHelper
    {
        public static DragMoveHelper Instance { get; } = new DragMoveHelper();

        // 关键：公开一个 ICommand，而不是方法
        public ICommand DragMoveCommand => new TangdaoCommand(() =>
        {
            Application.Current.Windows.OfType<Window>()
                       .SingleOrDefault(w => w.IsActive)?.DragMove();
        });

        public ICommand CreateDragMoveCommand()
        {
            return new TangdaoCommand(() =>
                Application.Current.Windows.OfType<Window>()
                           .SingleOrDefault(w => w.IsActive)?.DragMove());
        }

        public void DragMove()
        {
            Application.Current.Windows.OfType<Window>()
                           .SingleOrDefault(w => w.IsActive)?.DragMove();
        }

        public string DragMove2()
        {
            return "系统登录";
        }
    }
}