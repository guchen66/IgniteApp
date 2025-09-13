using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteApp.ViewModels;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.IServices;
using IT.Tangdao.Framework.Helpers;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class UserInfoViewModel : ViewModelBase
    {
        private BindableCollection<LoginDto> _loginInfos;

        public BindableCollection<LoginDto> LoginInfos
        {
            get => _loginInfos;
            set => SetAndNotify(ref _loginInfos, value);
        }

        public IWindowManager windowManager;
        public IReadService readService;

        public UserInfoViewModel(IWindowManager windowManager, IReadService readService)
        {
            this.windowManager = windowManager;
            this.readService = readService;

            ShowUserInfo();
        }

        public void ShowUserInfo()
        {
            var xmlData = readService.Read(Path.Combine(IgniteInfoLocation.User, "UserInfo.xml"));
            XDocument doc = XDocument.Parse(xmlData);
            List<LoginDto> loginList = doc.Descendants("Login")
           .Select(login => new LoginDto
           {
               // Id = (int)login.Attribute("Id"),
               UserName = (string)login.Element("UserName"),
               Password = (string)login.Element("Password"),
               Role = (RoleType)Enum.Parse(typeof(RoleType), (string)login.Element("Role")),
               IsAdmin = (bool)login.Element("IsAdmin"),
               IP = (string)login.Element("IP")
           })
           .ToList();
            //Descendants选择文档所有名称是Login的元素
            //var ips = doc.Descendants("Login").Select(node => node.Element("IP").Value).ToList();
            LoginInfos = new BindableCollection<LoginDto>(loginList);
        }

        public void GenerateReport()
        {
            List<LoginDto> loginDtos = new List<LoginDto>();
            loginDtos.Where(x => x.IP == "1");
            loginDtos.FirstOrDefault(x => x.IP == "");
        }
    }
}