using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IgniteApp.Shell.Set.Models
{
    public class SystemMenuItem:DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _type;

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private IList _items;

        public IList Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

     

    }
}
