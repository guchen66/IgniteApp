using IgniteApp.Shell.Home.Models;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Results;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.DaoParameters.Infrastructure;
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
        public static ReadOnlyCollection<ITangdaoMenuItem> Create(IReadService readService, string readTitle)
        {
            //字典转列表
            var result = readService.Current.SelectConfig(readTitle).ToReadResult<Dictionary<string, string>>().Data;

            if (result is Dictionary<string, string> d1)
            {
                var lists = d1.TryOrderBy().Select((kvp, index) => new TangdaoMenuItem
                {
                    Id = index + 1,
                    MenuName = kvp.Value,
                }).ToList();
                return new ReadOnlyCollection<ITangdaoMenuItem>(lists.Cast<ITangdaoMenuItem>().ToList());
            }
            else
            {
                return new ReadOnlyCollection<ITangdaoMenuItem>(new List<ITangdaoMenuItem>());
            }
        }

        public static IReadOnlyCollection<ITangdaoMenuItem> Create2<T>(IReadService readService, string readTitle)
        {
            var result = readService.Current.SelectConfig(readTitle).ToReadResult<Dictionary<string, string>>().Data;

            if (result is Dictionary<string, T> dict)
            {
                return dict.TryOrderBy()
                    .Select(kvp => new TangdaoMenuItem { })
                    .ToList();
            }
            else
            {
                return Array.Empty<ITangdaoMenuItem>(); // 或 new List<ITangdaoMenuItem>()
            }
        }

        public static IReadOnlyCollection<HomeMenuItem> Create(IReadService readService, string readTitle, string section)
        {
            var s1 = readService.Current.SelectCustomConfig(readTitle, section);
            var s2 = s1.ToReadResult<Dictionary<string, string>>();
            var dicts = readService.Current.SelectCustomConfig(readTitle, section).ToReadResult<Dictionary<string, string>>().Data;

            //   var model = readService.Current.SelectCustomConfig(readTitle, section).ToGenericResult<Dictionary<string, string>>().Data;
            if (dicts != null)
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
                return Array.Empty<HomeMenuItem>(); // 或 new List<ITangdaoMenuItem>()
            }
        }
    }
}