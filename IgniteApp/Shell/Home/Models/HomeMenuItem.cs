using IgniteApp.Common;
using IT.Tangdao.Framework.Extensions;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public struct HomeMenuItem
    {
        public string Title { get; set; }

        public string ViewName { get; set; }

        public IScreen View { get; set; }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetItem()
        {
            this=new HomeMenuItem();
        }
    }

    public static class HomeMenuItemExtension
    {
        /// <summary>
        /// 读取WPF自带的App.config
        /// 这两个引用没有传递值，是读取config的值，所以不需要使用ref，
        /// 使用了struct后，如果传递数据的扩展方法，需要加上ref
        /// </summary>
        /// <param name="menuList"></param>
        public static Dictionary<string, string> ReadAppConfig(this HomeMenuItem menuList, string section)
        {
            IDictionary idict = (IDictionary)ConfigurationManager.GetSection(section);
            Dictionary<string, string> dict = idict.Cast<DictionaryEntry>().ToDictionary(de => de.Key.ToString(), de => de.Value.ToString());
            return dict;
        }

        /// <summary>
        /// 读取自定义的config文件
        /// </summary>
        /// <param name="menuList"></param>
        public static Dictionary<string, string> ReadUnityConfig(this HomeMenuItem menuList, string section)
        {
            Dictionary<string, string> dicts = new Dictionary<string, string>();
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"unity.config")
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var customSection = (MenuConfiguration)config.GetSection(section);
            if (customSection==null)
            {
                dicts.Add("null",null);
                return dicts;
            }
            foreach (MenuElement menu in customSection.Menus)
            {
                dicts.TryAdd(menu.Title, menu.Value);
            }
            return dicts;

        }
    }
}
