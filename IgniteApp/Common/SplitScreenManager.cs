using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Screen = System.Windows.Forms.Screen;

namespace IgniteApp.Common
{
    public class SplitScreenManager
    {
        private void PositionMainWindow()
        {
            // 获取所有屏幕
            var screens = Screen.AllScreens;

            if (screens.Length > 1)
            {
                // 假设主屏幕是第一个屏幕，副屏幕是第二个屏幕
                var primaryScreen = screens[0];
                var secondaryScreen = screens[1];

                // 将主窗口定位到主屏幕
                var Left = primaryScreen.WorkingArea.Left + 100;
                var Top = primaryScreen.WorkingArea.Top + 100;
            }
            // 如果只有一个屏幕，使用默认位置
        }

        /// <summary>
        /// 在多屏幕环境中打开窗口，优先在副屏幕显示
        /// </summary>
        /// <param name="windowManager">Stylet窗口管理器</param>
        /// <param name="viewModel">要显示的ViewModel</param>
        /// <param name="primaryScreenIndex">主屏幕索引（默认0）</param>
        /// <param name="secondaryScreenIndex">副屏幕索引（默认1）</param>
        public static void OpenOnSecondaryScreen(IWindowManager windowManager, object viewModel,
            int primaryScreenIndex = 0, int secondaryScreenIndex = 1)
        {
            var screens = Screen.AllScreens;

            if (screens.Length <= 1)
            {
                // 单屏幕环境，正常显示
                windowManager.ShowWindow(viewModel);
                return;
            }

            // 确保屏幕索引有效
            secondaryScreenIndex = secondaryScreenIndex < screens.Length ? secondaryScreenIndex : screens.Length - 1;

            // 获取副屏幕
            var secondaryScreen = screens[secondaryScreenIndex];

            // 显示窗口
            windowManager.ShowWindow(viewModel);

            // 获取并定位窗口
            var window = GetWindowFromViewModel(viewModel);
            if (window != null)
            {
                // 将窗口定位到副屏幕的左上角
                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Left = secondaryScreen.WorkingArea.Left;
                window.Top = secondaryScreen.WorkingArea.Top;

                // 可选：设置窗口大小为副屏幕工作区的一定比例
                window.Width = secondaryScreen.WorkingArea.Width * 0.8;
                window.Height = secondaryScreen.WorkingArea.Height * 0.7;
            }
        }

        /// <summary>
        /// 从ViewModel获取对应的Window对象
        /// </summary>
        private static Window GetWindowFromViewModel(object viewModel)
        {
            // 通过Application.Current.Windows查找对应的窗口
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.DataContext == viewModel)
                {
                    return window;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取屏幕信息字符串（用于调试或显示）
        /// </summary>
        public static string GetScreensInfo()
        {
            var screens = Screen.AllScreens;
            return string.Join("\n", screens.Select((screen, index) =>
                $"屏幕 {index}: {screen.Bounds.Width}x{screen.Bounds.Height} " +
                $"{(screen.Primary ? "(主屏幕)" : "(副屏幕)")}"));
        }

        /// <summary>
        /// 获取指定索引的屏幕工作区域
        /// </summary>
        public static Rect GetScreenWorkingArea(int screenIndex = 0)
        {
            var screens = Screen.AllScreens;
            if (screenIndex >= screens.Length)
                screenIndex = screens.Length - 1;

            var screen = screens[screenIndex];
            return new Rect(
                screen.WorkingArea.Left,
                screen.WorkingArea.Top,
                screen.WorkingArea.Width,
                screen.WorkingArea.Height);
        }
    }
}