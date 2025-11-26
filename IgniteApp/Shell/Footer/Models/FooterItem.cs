using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Mvvm;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Footer.Models
{
    public class FooterItem
    {
        public string Title { get; set; }
    }

    //public class FooterList: DaoViewModelBase,IDaoCloneable<FooterList>
    //{
    //    private BindableCollection<FooterItem> _items;

    //    public BindableCollection<FooterItem> Items
    //    {
    //        get => _items;
    //        set => SetProperty(ref _items, value);
    //    }

    //    public FooterList Clone()
    //    {
    //        // 创建一个新的 FooterList 对象
    //        var clonedList = new FooterList();

    //        // 创建一个新的 BindableCollection<FooterItem> 实例
    //        clonedList.Items = new BindableCollection<FooterItem>();

    //        // 为新的集合添加原始集合中每个元素的副本
    //        foreach (var item in this.Items)
    //        {
    //            clonedList.Items.Add(new FooterItem { Title = item.Title });
    //        }

    //        return clonedList;
    //    }
    //}
}