using IgniteApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class CommonSetViewModel:ControlViewModelBase
    {
		private string[] _languages;

		public string[] Languages
        {
			get => _languages;
			set => SetAndNotify(ref _languages, value);
		}

		private string _language;

		public string Language
        {
			get => _language;
			set => SetAndNotify(ref _language, value);
		}

		public CommonSetViewModel()
        {
            Languages = new string[] { "English", "汉语", "한국어", "русский" };
            // 设置默认选中值
            Language = Languages[1]; // 默认选中English
        }
    }
}
