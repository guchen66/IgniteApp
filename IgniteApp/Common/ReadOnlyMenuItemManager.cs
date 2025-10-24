using IgniteApp.Shell.Home.Models;
using IT.Tangdao.Framework.Abstractions.Results;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.EventArg;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace IgniteApp.Common
{
    public class ReadOnlyMenuItemManager
    {
        //public static ReadOnlyCollection<TangdaoMenuItem> Create(IReadService readService, string readTitle)
        //{
        //   return readService.Default.AsConfig().SelectAppConfig(readTitle).ToList(v => new TangdaoMenuItem { MenuName = v }).ToObservableCollection();
        //   // var dict = readService.Default.AsConfig().SelectAppConfig(readTitle).ToReadResult<TangdaoSortedDictionary<string, string>>().Data;
        //   // var list = dict.Values.Select(v => new TangdaoMenuItem { MenuName = v }).ToList();
        //   // return new ReadOnlyCollection<TangdaoMenuItem>(list);
        //}

        //public static IReadOnlyCollection<ITangdaoMenuItem> Create2<T>(IReadService readService, string readTitle)
        //{
        //    var result = readService.Default.AsConfig().SelectAppConfig(readTitle).ToReadResult<Dictionary<string, string>>().Data;

        //    if (result is Dictionary<string, T> dict)
        //    {
        //        return dict.TryOrderBy()
        //            .Select(kvp => new TangdaoMenuItem { })
        //            .ToList();
        //    }
        //    else
        //    {
        //        return Array.Empty<ITangdaoMenuItem>(); // 或 new List<ITangdaoMenuItem>()
        //    }
        //}

        public static IReadOnlyCollection<HomeMenuItem> Create(IReadService readService, string readTitle, string section)
        {
            var responseResult = readService.Default.AsConfig().SelectCustomConfig(readTitle, section);
            //var s2 = s1.ToGenericResult<Dictionary<string, string>>();
            var dicts = readService.Default.AsConfig().SelectCustomConfig(readTitle, section).ToGenericResult<Dictionary<string, string>>().Data;

            //   var model = readService.Default.SelectCustomConfig(readTitle, section).ToGenericResult<Dictionary<string, string>>().Data;

            if (responseResult.Payload is Dictionary<string, string> data)
            {
                List<HomeMenuItem> menuItems = data.Select(kvp => new HomeMenuItem
                {
                    Title = kvp.Key,
                    ViewModelName = kvp.Value
                }).ToList();
                return new ReadOnlyCollection<HomeMenuItem>(menuItems);
            }
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