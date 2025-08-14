using IgniteApp.Shell.Home.Models;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoDtos.Items;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
    public class ReadOnlyMenuItemManager
    {
        public static ReadOnlyCollection<IMenuItem> Create(IReadService readService, string readTitle)
        {
            //字典转列表
            var result = readService.Current.SelectConfig(readTitle).Result;

            if (result is Dictionary<string, string> d1)
            {
                var lists = d1.TryOrderBy().Select((kvp, index) => new TangdaoMenuItem
                {
                    Id = index + 1,
                    MenuName = kvp.Value,
                }).ToList();
                return new ReadOnlyCollection<IMenuItem>(lists.Cast<IMenuItem>().ToList());
            }
            else
            {
                return new ReadOnlyCollection<IMenuItem>(new List<IMenuItem>());
            }
        }

        public static IReadOnlyCollection<IMenuItem> Create2<T>(IReadService readService, string readTitle)
        {
            var result = readService.Current.SelectConfig(readTitle).Result;

            if (result is Dictionary<string, T> dict)
            {
                return dict.TryOrderBy()
                    .Select(kvp => new TangdaoMenuItem { })
                    .ToList();
            }
            else
            {
                return Array.Empty<IMenuItem>(); // 或 new List<IMenuItem>()
            }
        }

        public static IReadOnlyCollection<HomeMenuItem> Create(IReadService readService, string readTitle, string section)
        {
            var model = readService.Current.SelectCustomConfig(readTitle, section).Result;
            if (model is Dictionary<string, string> dicts)
            {
                List<HomeMenuItem> menuItems = dicts.Select(kvp => new HomeMenuItem
                {
                    Title = kvp.Key,
                    ViewModelName = kvp.Value
                }).ToList();
                return new ReadOnlyCollection<HomeMenuItem>(menuItems);
                // HomeMenuItems.AddRange(menuItems);
            }
            else
            {
                return Array.Empty<HomeMenuItem>(); // 或 new List<IMenuItem>()
            }
        }
    }
}