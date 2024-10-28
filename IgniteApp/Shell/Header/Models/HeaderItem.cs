using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Header.Models
{
    public struct HeaderItem
    {
        public string Title { get; set; }

        public void Reset()
        {
            this=new HeaderItem();
        }
    }

    public class HeaderItemProvider
    {
        private readonly Dictionary<string, HeaderItem> Items;

        private HeaderItemProvider()
        {
            Items = new Dictionary<string, HeaderItem>();
        }

        private static readonly Lazy<HeaderItemProvider> _instance=new Lazy<HeaderItemProvider>(() => new HeaderItemProvider());
        public static HeaderItemProvider Instance=_instance.Value;

        public void AddHeaderItem(HeaderItem headerItem)
        {
            Items.TryAdd(headerItem.Title, headerItem);
        }

        public HeaderItem GetHeaderItem(string title)
        {
            if (Items.ContainsKey(title))
            {
                Items.TryGetValue(title, out HeaderItem headerItem);
                return headerItem;
            }
            return default;
        }
    }
}
