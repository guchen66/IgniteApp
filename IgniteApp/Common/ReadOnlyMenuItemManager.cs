using IT.Tangdao.Framework.DaoAdmin.IServices;
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
                var lists = d1.TryOrderBy().Select(kvp => new TangdaoMenuItem
                {
                    MenuName = kvp.Value,
                }).ToList();
                return new ReadOnlyCollection<IMenuItem>(lists.Cast<IMenuItem>().ToList());
            }
            else
            {
                return new ReadOnlyCollection<IMenuItem>(new List<IMenuItem>());
            }
        }

        public static IReadOnlyCollection<IMenuItem> Create2(IReadService readService, string readTitle)
        {
            var result = readService.Current.SelectConfig(readTitle).Result;

            if (result is Dictionary<string, string> dict)
            {
                return dict.TryOrderBy()
                    .Select(kvp => new TangdaoMenuItem { MenuName = kvp.Value })
                    .ToList();
            }
            else
            {
                return Array.Empty<IMenuItem>(); // 或 new List<IMenuItem>()
            }
        }
    }
}