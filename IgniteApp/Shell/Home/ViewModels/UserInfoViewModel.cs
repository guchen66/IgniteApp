using HandyControl.Controls;
using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteApp.ViewModels;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Helpers;
using IT.Tangdao.Xaml.Controls;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

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

        private string _password;

        public string Password
        {
            get => _password;
            set => SetAndNotify(ref _password, value);
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        public IWindowManager windowManager;
        public IReadService readService;
        public ICommand UnlockCommand { get; set; }

        public UserInfoViewModel(IWindowManager windowManager, IReadService readService)
        {
            this.windowManager = windowManager;
            this.readService = readService;
            UnlockCommand = new TangdaoCommand<string>(ExecuteUnlock);
            ShowUserInfo();
        }

        public void ShowUserInfo()
        {
            ObservableCollection<int> sss = new ObservableCollection<int>();
            var xmlData = readService.Default.Read(Path.Combine(IgniteInfoLocation.User, "UserInfo.xml")).Content;
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

        private void ExecuteUnlock(string name)
        {
            if (Password == "123")
            {
                IsEnabled = true;
            }
            else
            {
                MessageBox.Error($"密码:{Password}错误");
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            IsEnabled = false;
            //Text = null;
        }
    }
}