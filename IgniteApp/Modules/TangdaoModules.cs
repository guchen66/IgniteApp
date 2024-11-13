using AutoMapper;
using IgniteAdmin.Providers;
using IgniteApp.Interfaces;
using IgniteShared.Models;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoAdmin.IServices;
using IT.Tangdao.Framework.DaoAdmin.Services;
using IT.Tangdao.Framework.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Extensions;

namespace IgniteApp.Modules
{
    public class TangdaoModules : StyletIoCModule
    {
        protected override void Load()
        {
            InitPlc();
            Bind<IWriteService>().To<WriteService>();
            Bind<IReadService>().To<ReadService>();
            Bind<IPlcProvider>().To<PlcProvider>().InSingletonScope();
            Bind<IPlcBuilder>().ToFactory(Builder);
        }

        private IPlcBuilder Builder(IContainer container)
        {
            InitPlc();
            var provider = container.Get<IPlcProvider>();
            return provider.Builder();
        }
        /// <summary>
        /// 是否初始化数据表
        /// </summary>
        /// <returns></returns>
        private void InitPlc()
        {
            var path = DirectoryHelper.SelectDirectoryByName("appsetting.json");
            string jsonContent = File.ReadAllText(path);
            var plcConfig = JsonConvert.DeserializeObject<PlcConfig>(JObject.Parse(jsonContent)["PlcConfig"].ToString());
            SysPlcInfo.IP=plcConfig.IP;
            SysPlcInfo.Port=plcConfig.Port.ToInt();
        }
    }
}
