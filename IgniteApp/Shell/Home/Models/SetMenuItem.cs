using IgniteApp.Common;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoMvvm;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public class SetMenuItem:DaoViewModelBase,IMenuItem
    {
		private string _setMenuName;

		public string SetMenuName
        {
			get => _setMenuName;
			set => SetProperty(ref _setMenuName, value);
		}

        private string _setMenuToView;

        public string SetMenuToView
        {
            get => _setMenuToView;
            set => SetProperty(ref _setMenuToView, value);
        }
        public string MenuName { get ; set ; }
    }

    public static class ReadItemExtension
    {
        /// <summary>
        /// 读取WPF自带的App.config
        /// 这两个引用没有传递值，是读取config的值，所以不需要使用ref，
        /// 使用了struct后，如果传递数据的扩展方法，需要加上ref
        /// 由于它是哈希列表，不保证顺序
        /// </summary>
        /// <param name="menuList"></param>
        public static Dictionary<string, string> ReadAppConfigToDic<TSource>(this TSource source, string section)
        {
            IDictionary idict = (IDictionary)ConfigurationManager.GetSection(section);
            if (idict==null)
            {
                throw new ContextMarshalException() ;
            }
            Dictionary<string, string> dict = idict.Cast<DictionaryEntry>().ToDictionary(de => de.Key.ToString(), de => de.Value.ToString());
            var sortedDicts = dict.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return sortedDicts;
        }

        public static OrderedDictionary ReadAppConfigToOrderDic<TSource>(this TSource source, string section)
        {
            IDictionary idict = (IDictionary)ConfigurationManager.GetSection(section);
            OrderedDictionary orderedDict = new OrderedDictionary();
            foreach (DictionaryEntry de in idict)
            {
                orderedDict[de.Key] = de.Value;
            }
            return orderedDict;
        }
    }
}
