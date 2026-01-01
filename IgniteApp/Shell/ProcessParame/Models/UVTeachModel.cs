using IT.Tangdao.Framework.Attributes;
using IT.Tangdao.Framework.Faker;
using IT.Tangdao.Framework.Mvvm;
using System.Xml.Serialization;

namespace IgniteApp.Shell.ProcessParame.Models
{
    [XmlRoot("UVTeachModelList")]
    public class UvTeachModel : DaoViewModelBase
    {
        private int _id;

        [TangdaoFake(PrimarykeyAutoIncrement = true)]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private double _startValue;

        public double StartValue
        {
            get => _startValue;
            set
            {
                SetProperty(ref _startValue, value);
            }
        }

        private double _endValue;

        public double EndValue
        {
            get => _endValue;
            set
            {
                SetProperty(ref _endValue, value);
            }
        }

        private string _cutTYpe;

        [TangdaoFake(Template = MockTemplate.City)]
        public string CutType
        {
            get => _cutTYpe;
            set => SetProperty(ref _cutTYpe, value);
        }

        private bool _isEdit;

        [TangdaoFake()]
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum
    {
        None, One, Two, Three, Four, Five,
    }
}