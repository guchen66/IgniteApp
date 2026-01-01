using IgniteApp.ViewModels;
using System.Windows;

namespace IgniteApp.Views
{
    /// <summary>
    /// RegisterView.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            this.DataContext = ServiceLocator.GetService<RegisterViewModel>();
        }
    }
}