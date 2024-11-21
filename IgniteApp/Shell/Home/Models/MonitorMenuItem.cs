using IgniteApp.Common;
using IgniteApp.Interfaces;
using IT.Tangdao.Framework.DaoMvvm;
using IT.Tangdao.Framework.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public class MonitorMenuItem : DaoViewModelBase, IMenuItem
    {
        public int Id { get; set; }
        public string MenuName { get; set; }

    }

    public static class MonitorItemExtension
    {
        /// <summary>
        /// 读取自定义的config文件
        /// </summary>
        /// <param name="menuList"></param>
        public static Dictionary<string, string> ReadUnityConfig(this MonitorMenuItem menuList, string section)
        {
           
            Dictionary<string, string> dicts = new Dictionary<string, string>();
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"unity.config")
            };
            
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            
            var customSection = config.GetSection(section) as IDictionary;
          /*  var s1=customSection.CurrentConfiguration;
            var s2=customSection.SectionInformation;
            var s3=customSection.ElementInformation;*/
          /*  foreach (MenuElement menu in )
            {
                dicts.TryAdd(menu.Title, menu.Value);
            }*/
            return dicts;

        }
        public static Dictionary<string,string> ReadUnityConfigToDict<TSource>(this TSource source,string section)
        {
            IDictionary idict = (IDictionary)ConfigurationManager.GetSection(section);
            Dictionary<string, string> dict = idict.Cast<DictionaryEntry>().ToDictionary(de => de.Key.ToString(), de => de.Value.ToString());
            return dict;
        }

        public static string[] ReadUnityConfigToNameValue<TSource>(this TSource source, string section)
        {
            NameValueCollection nameValues = (NameValueCollection)ConfigurationManager.GetSection(section);
     
            string[] values = nameValues.AllKeys.SelectMany(key => nameValues.GetValues(key)).ToArray();
            return values;
        }
    }
}
