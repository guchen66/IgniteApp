using IgniteShared.Dtos;
using IgniteShared.Entitys;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IgniteAdmin.Managers.Login
{
    public class UserManager
    {
        public static void SaveUserInfo(LoginDto loginDto)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement userElement;
            var filePath = Path.Combine(IgniteInfoLocation.User, "UserInfo.xml");
            if (File.Exists(filePath))
            {
                xmlDocument.Load(filePath);
                userElement = xmlDocument.DocumentElement;
            }
            else
            {
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDocument.AppendChild(xmlDeclaration);
                userElement = xmlDocument.CreateElement("UserInfo");
                xmlDocument.AppendChild(userElement);
            }
            // 假设userElement是已经加载的XmlElement
            // 将XmlNode转换为XElement
            XElement xUserElement = XElement.Parse(userElement.OuterXml);

            // 获取所有IP地址
            var ipAddresses = xUserElement.Elements("Login")
                                        .Select(loginElement => loginElement.Element("IP")?.Value);
            // 在一次查询中同时获取用户名和密码
            var loginInfo = xUserElement.Elements("Login")
                                       .Select(loginElement => new
                                       {
                                           UserName = loginElement.Element("UserName")?.Value,
                                           Password = loginElement.Element("Password")?.Value
                                       })
                                       .ToList();

            // 检查用户是否已存在
            bool userExists = loginInfo.Any(info => info.UserName == loginDto.UserName && info.Password == loginDto.Password);

            if (!userExists)
            {
                //给userInfo.xml添加子节点
                //一级节点
                int loginCount = userElement.ChildNodes.Count;
                XmlElement Login = xmlDocument.CreateElement("Login");
                Login.SetAttribute("Id", loginCount.ToString());
                //二级节点
                XmlElement UserName = xmlDocument.CreateElement("UserName");
                UserName.InnerText = loginDto.UserName;
                Login.AppendChild(UserName);

                XmlElement Password = xmlDocument.CreateElement("Password");
                Password.InnerText = loginDto.Password;
                Login.AppendChild(Password);

                XmlElement Role = xmlDocument.CreateElement("Role");
                Role.InnerText = loginDto.Role.ToString();
                Login.AppendChild(Role);

                XmlElement IsAdmin = xmlDocument.CreateElement("IsAdmin");
                IsAdmin.InnerText = loginDto.IsAdmin.ToString();
                Login.AppendChild(IsAdmin);

                XmlElement IP = xmlDocument.CreateElement("IP");
                IP.InnerText = loginDto.IP;
                Login.AppendChild(IP);

                userElement.AppendChild(Login);
            }
            xmlDocument.Save(filePath);
        }

        public static bool SearchCache(LoginDto loginDto)
        {
            var localLoginData = Path.Combine(IgniteInfoLocation.User, "UserInfo.xml");
            XElement xElement = XElement.Load(localLoginData);
            List<XElement> xElements = xElement.Descendants().ToList();
            bool isValidUser = xElement.Descendants("Login")
                                .Any(login =>
                                    (string)login.Element("UserName") == loginDto.UserName &&
                                    (string)login.Element("Password") == loginDto.Password);
            return isValidUser;
        }
    }

    public static class LoginManager
    {
        private static readonly Lazy<LoginDto> _instance = new Lazy<LoginDto>(() => new LoginDto());
        public static LoginDto Instance = _instance.Value;
    }
}