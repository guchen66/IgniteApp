using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Dtos
{
    public class RecipeDto:DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _recipeName;

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }
        private string _remark;

        public string Remark
        {
            get => _remark;
            set => SetProperty(ref _remark, value);
        }

        private string _viewName;

        public string ViewName
        {
            get => _viewName;
            set => SetProperty(ref _viewName, value);
        }

        private DateTime _createTime;

        public DateTime CreateTime
        {
            get => _createTime;
            set => SetProperty(ref _createTime, value);
        }
        private DateTime _updateTime;

        public DateTime UpdateTime
        {
            get => _updateTime;
            set => SetProperty(ref _updateTime, value);
        }

        private bool _isDeleted;

        public bool IsDeleted
        {
            get => _isDeleted;
            set => SetProperty(ref _isDeleted, value);
        }
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}
