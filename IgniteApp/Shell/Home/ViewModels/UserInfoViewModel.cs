using HandyControl.Controls;
using IgniteAdmin.Managers.Login;
using IgniteApp.Bases;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.ViewModels;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Enums;
using IgniteShared.Globals.Local;
using IgniteShared.Globals.System;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using IT.Tangdao.Framework.Markup;
using IT.Tangdao.Xaml.Controls;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using ColorConverter = System.Windows.Media.ColorConverter;

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

        private BindableCollection<LoginDto> _fakeloginInfos;

        public BindableCollection<LoginDto> FakeLoginInfos
        {
            get => _fakeloginInfos;
            set => SetAndNotify(ref _fakeloginInfos, value);
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

        private Brush _textColor = Brushes.Red; //new SolidColorBrush(Colors.Red);

        public Brush TextColor
        {
            get => _textColor;
            set => SetAndNotify(ref _textColor, value);
        }

        public IWindowManager windowManager;
        public IContentReader readService;
        public ICommand UnlockCommand { get; set; }
        public ICommand GetCellInfoCommand { get; set; }
        public ICommand GetIlistCellInfoCommand { get; set; }
        private readonly ITangdaoLogger Logger = TangdaoLogger.Get(typeof(UserInfoViewModel));

        public UserInfoViewModel(IWindowManager windowManager, IContentReader readService)
        {
            this.windowManager = windowManager;
            this.readService = readService;
            UnlockCommand = new TangdaoCommand<string>(ExecuteUnlock);
            GetCellInfoCommand = new TangdaoCommand<DataGridCellInfo>(ExecuteGetCellInfo);
            GetIlistCellInfoCommand = new TangdaoCommand<IList<DataGridCellInfo>>(ExecuteIListGetCellInfo);
            ShowUserInfo();
        }

        private void ExecuteGetCellInfo(DataGridCellInfo info)
        {
        }

        private void ExecuteIListGetCellInfo(IList<DataGridCellInfo> info)
        {
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

            var tangdaoDataFaker = new TangdaoDataFaker<LoginDto>();
            var lists = tangdaoDataFaker.Build(200);
            FakeLoginInfos = new BindableCollection<LoginDto>(lists);
            // NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs();
        }

        public void GenerateReport()
        {
            List<LoginDto> loginDtos = new List<LoginDto>();
            loginDtos.Where(x => x.IP == "1");
            loginDtos.FirstOrDefault(x => x.IP == "");
        }

        private void ExecuteUnlock(string name)
        {
            var uri = PackUriProvider.UriParses;
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