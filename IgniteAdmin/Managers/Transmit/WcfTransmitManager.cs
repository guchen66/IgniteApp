using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteAdmin.Managers.Transmit
{
    public class WcfTransmitManager
    {
        public static async Task<bool> StartWcfAsync()
        {
           var json= await JsonConverHelper.GetDecisionJsonAsync("appsetting.json","WCF");

            var model = JsonConvert.DeserializeObject<WCFTransmitData>(json);
            return model.Startup;
        }

        public static bool StartWcf()
        {
            var json =  GetDecisionJson("appsetting.json", "WCF");

            var model = JsonConvert.DeserializeObject<WCFTransmitData>(json);
            return model.Startup;
        }

        /// <summary>
        /// 获取根目录下的指定json文件并打开查看内容
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="key">JSON对象中的键</param>
        /// <returns>JSON对象中对应键的值</returns>
        public static string GetDecisionJson(string resourceName, string key)
        {
            var path = DirectoryHelper.SelectDirectoryByName(resourceName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("指定的文件未找到。", path);
            }

            using (var stream = File.OpenText(path))
            {
                if (stream == null) return null;

                JsonTextReader reader = new JsonTextReader(stream);
                JObject jsonObject = (JObject)JToken.ReadFrom(reader);

                string json = jsonObject[key]?.ToString();
                return json;
            }
        }
    }

    public class WCFTransmitData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Startup {  get; set; }
    }
}
