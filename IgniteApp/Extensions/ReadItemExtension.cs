using IgniteApp.Bases;
using IgniteApp.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Extensions
{
    public static class ReadItemExtension
    {
        /// <summary>
        /// 读取WPF自带的App.config
        /// 这两个引用没有传递值，是读取config的值，所以不需要使用ref，
        /// 使用了struct后，如果传递数据的扩展方法，需要加上ref
        /// 由于它是哈希列表，不保证顺序
        /// </summary>
        /// <param name="menuList"></param>
        public static Dictionary<string, string> ReadAppConfigToDic<TSource>(this TSource source, string section)where TSource : IAppConfigProvider
        {
            IDictionary idict = (IDictionary)ConfigurationManager.GetSection(section);
            if (idict == null)
            {
                throw new ContextMarshalException();
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
