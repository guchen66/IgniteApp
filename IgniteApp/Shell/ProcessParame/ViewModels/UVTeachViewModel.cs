using IgniteApp.Behaviors;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.ViewModels;
using IT.Tangdao.Framework;
using IT.Tangdao.Framework.Abstractions;
using IT.Tangdao.Framework.Abstractions.Loggers;
using IT.Tangdao.Framework.Abstractions.Navigates;
using IT.Tangdao.Framework.Commands;
using IT.Tangdao.Framework.DaoTasks;
using IT.Tangdao.Framework.Enums;
using IT.Tangdao.Framework.Extensions;
using IT.Tangdao.Framework.Helpers;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public UVTeachViewModel()
        {
            TangdaoDataFaker<UvTeachModel> tangdaoDataFaker = new TangdaoDataFaker<UvTeachModel>();
            TangdaoTaskScheduler.Execute((dao) =>
            {
                StoreProvider = tangdaoDataFaker.Build(2000000);
                Logger.WriteLocal($"耗时：{dao.Duration}");
            }, TaskThreadType.UI);
            UvTeachModelList = StoreProvider.ToObservableCollection();
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

        public void SaveData()
        {
            // 一行 LINQ 搞定：把列表里所有元素的 IsEdit 统一设成 false，其余属性原封不动
            //UvTeachModelList=UvTeachModelList.ForEach(m => m.IsEdit = false);
            // UvTeachModelList = UvTeachModelList.TryForEach(m => m.IsEdit = true).ToObservableCollection();
            //UvTeachModelList = UvTeachModelList
            //       .Select(m => { m.IsEdit = false; return m; })
            //       .ToObservableCollection();
            UvTeachModelList = new ObservableCollection<UvTeachModel>(UvTeachModelList);
            // UvTeachModelList = UvTeachModelList.ToList().ToObservableCollection();
            // OnPropertyChanged(nameof(UvTeachModelList));
        }

        public ICommand SaveDataCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
    }
}