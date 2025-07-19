using IgniteApp.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.ViewModels
{
    public class AssistSetViewModel:ViewModelBase
    {
		/// <summary>
		/// 高对比度
		/// </summary>
		private string _contrast;

		public string Contrast
        {
			get => _contrast;
			set => SetAndNotify(ref _contrast, value);
		}

		private string _size;

		public string Size
		{
			get => _size;
			set => SetAndNotify(ref _size, value);
		}

		public AssistSetViewModel()
        {
            Contrast = "100";
			Size = "12";
        }
    }
}
