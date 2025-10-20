using IgniteApp.Shell.Set.Views;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IgniteApp.Interfaces
{
    public class SystemSetViewFactory
    {
        public static Dictionary<int, UserControl> Views = new Dictionary<int, UserControl>();

        public static UserControl CreateView(int id)
        {
            // 先尝试从字典中获取已存在的实例
            if (Views.TryGetValue(id, out UserControl screen))
            {
                return screen;
            }

            // 如果不存在，根据id创建新的实例
            switch (id)
            {
                case 1:
                    screen = new CommonSetView();
                    break;

                case 2:
                    screen = new AccountSetView();
                    break;

                case 3:
                    screen = new NetSetView();
                    break;

                case 4:
                    screen = new HardawreSetView();
                    break;

                case 5:
                    screen = new AssistSetView();
                    break;

                default:
                    throw new ArgumentException($"未知的ID: {id}");
            }

            // 将新创建的实例存入字典
            Views[id] = screen;
            return screen;
        }

        // 可选：添加清理方法
        public static void ClearCache()
        {
            Views.Clear();
        }

        // 可选：移除特定视图
        public static bool RemoveView(int id)
        {
            return Views.Remove(id);
        }
    }
}