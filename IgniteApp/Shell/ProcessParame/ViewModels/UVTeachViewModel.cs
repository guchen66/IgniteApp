using IgniteApp.Shell.ProcessParame.Models;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Navigation;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.DaoTasks;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Infrastructure;
using IT.Tangdao.Framework.Paths;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace IgniteApp.Shell.ProcessParame.ViewModels
{
    public class UVTeachViewModel : Screen, ITangdaoPage
    {
        public string PageTitle => "";
        private ObservableCollection<UvTeachModel> _uvTeachModelList;

        public ObservableCollection<UvTeachModel> UvTeachModelList
        {
            get => _uvTeachModelList;
            set => SetAndNotify(ref _uvTeachModelList, value);
        }

        /// <summary>
        /// 改变某一个单元格前景色
        /// </summary>
        private DataGridCellInfo _currentCell;

        public DataGridCellInfo CurrentCell
        {
            get => _currentCell;
            set
            {
                if (SetAndNotify(ref _currentCell, value) && value.IsValid)
                {
                    var cellContent = value.Column.GetCellContent(value.Item);
                    DataGridCell dataGridCell = (DataGridCell)cellContent?.Parent;
                    dataGridCell.Foreground = Brushes.Red;
                }
            }
        }

        public DataGrid OwnerGrid { get; set; }   // 由行为注入
        private int _currentPage = 1; // 当前页码
        private int _pageSize = 20;   // 每页显示数量
        private int _totalCount;      // 总数据量
        private List<UvTeachModel> StoreProvider { get; set; }
        public string PageInfo => $"第 {_currentPage} 页 / 共 {_pageCount} 页 (共 {_totalCount} 条记录)";
        public int PageCount => _pageCount;
        private int _pageCount; // 总页数
        private ITangdaoLogger Logger = TangdaoLogger.Get(typeof(UVTeachViewModel));
        private static int count = 1;

        public UVTeachViewModel()
        {
            TangdaoTaskScheduler.Execute((dao) =>
            {
                TangdaoDataFaker<UvTeachModel> tangdaoDataFaker = new TangdaoDataFaker<UvTeachModel>();
                StoreProvider = tangdaoDataFaker.Build(200000);
                Logger.WriteLocal($"第{count}次耗时：{dao.Elapsed}");
                UvTeachModelList = StoreProvider.ToObservableCollection();
                count++;
            });

            SaveDataCommand = new TangdaoCommand(SaveData);
            PrevPageCommand = new TangdaoCommand(ExecutePrevPage);
            NextPageCommand = new TangdaoCommand(ExecuteNextPage);
            _totalCount = StoreProvider.Count;
            _pageCount = (int)Math.Ceiling(_totalCount / (double)_pageSize);
            LoadCurrentPage();
        }

        // 加载当前页数据
        private void LoadCurrentPage()
        {
            var skip = (_currentPage - 1) * _pageSize;
            UvTeachModelList = StoreProvider.Skip(skip).Take(_pageSize).ToObservableCollection();

            // 更新界面显示的分页信息
            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(PageCount));
        }

        private void ExecutePrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCurrentPage();
            }
        }

        private void ExecuteNextPage()
        {
            if (_currentPage < _pageCount)
            {
                _currentPage++;
                LoadCurrentPage();
            }
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

        private void SaveData()
        {
            UvTeachModelList = new ObservableCollection<UvTeachModel>(UvTeachModelList);
            var sss1 = TangdaoPath.DateFrom(IgniteInfoLocation.Recipe).BuildDirectory();
            //var directoryPath = TangdaoPath.Instance.DateFrom(IgniteInfoLocation.Recipe).BuildFile("111.xml");

            var sss = TangdaoPath.AsPath(IgniteInfoLocation.Recipe);
            var directoryPath = TangdaoPath.AsPath(IgniteInfoLocation.Recipe).BuildFile("111.xml");
            SerializeXMLToFile(UvTeachModelList, directoryPath.Value);
        }

        public static void SerializeXMLToFile<T>(T obj, string filePath, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8; // 默认使用 UTF-8 编码
            }

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = encoding,
                Indent = true, // 可选：使 XML 更具可读性
                OmitXmlDeclaration = false // 确保包含 XML 声明
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
        }

        private class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding _encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                _encoding = encoding;
            }

            public override Encoding Encoding => _encoding;
        }

        public ICommand SaveDataCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
    }
}