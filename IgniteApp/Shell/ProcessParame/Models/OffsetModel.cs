using IT.Tangdao.Framework.Attributes;
using IT.Tangdao.Framework.Faker;
using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public class OffsetModel : DaoViewModelBase
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private bool _isXDirty;

        public bool IsXDirty
        {
            get => _isXDirty;
            set => SetProperty(ref _isXDirty, value);
        }

        private bool _isYDirty;

        public bool IsYDirty
        {
            get => _isYDirty;
            set => SetProperty(ref _isYDirty, value);
        }

        private double _startValue;

        public double StartValue
        {
            get => _startValue;
            set
            {
                SetProperty(ref _startValue, value);
                IsXDirty = true;
            }
        }

        private double _endValue;

        public double EndValue
        {
            get => _endValue;
            set
            {
                SetProperty(ref _endValue, value);
                IsYDirty = true;
            }
        }

        private string _cutTYpe;

        [TangdaoFake(Template = MockTemplate.City)]
        public string CutType
        {
            get => _cutTYpe;
            set => SetProperty(ref _cutTYpe, value);
        }
    }
}