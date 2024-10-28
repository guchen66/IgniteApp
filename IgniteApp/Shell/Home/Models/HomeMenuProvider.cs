using IgniteApp.Shell.Header.Models;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public class HomeMenuProvider
    {
        private readonly Dictionary<string, HomeMenuItem> Items;

        private HomeMenuProvider()
        {
            Items = new Dictionary<string, HomeMenuItem>();
        }

        private static readonly Lazy<HomeMenuProvider> _instance = new Lazy<HomeMenuProvider>(() => new HomeMenuProvider());
        public static HomeMenuProvider Instance = _instance.Value;

        public void AddHomeMenuItem(HomeMenuItem item)
        {
            Items.TryAdd(item.Title, item);
        }

        public HomeMenuItem GetHomeMenuItem(string title)
        {
            if (Items.ContainsKey(title))
            {
                Items.TryGetValue(title, out HomeMenuItem item);
                return item;
            }
            return default;
        }

        public List<HomeMenuItem> GetHomeMenus()
        {
           return Items.Select(x=>x.Value).ToList();
        }
    }
}
