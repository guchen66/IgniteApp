using IgniteApp.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Navigates;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class UVTeachViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "";
        //  private ITangdaoResponse _tangdaoResponse;

        public UVTeachViewModel()
        {
            // _tangdaoResponse = tangdaoResponse;
            //  _tangdaoResponse.Received += _tangdaoResponse_Received;
        }

        private string _responseData;

        public string ResponseData
        {
            get => _responseData;
            set => SetAndNotify(ref _responseData, value);
        }

        public bool CanNavigateAway()
        {
            return true;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo(ITangdaoParameter parameter = null)
        {
            ResponseData = parameter.Get<string>("Name");
        }

        [ScannerSubscribe("Hello")]
        public void Reponse()
        {
            Console.WriteLine("接收Hello——UVTeach");
        }

        private void _tangdaoResponse_Received(object sender, string e)
        {
            // ResponseData
        }
    }
}