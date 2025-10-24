using HandyControl.Controls;
using IgniteApp.Bases;
using IgniteApp.Common;
using IT.Tangdao.Framework.Commands;
using MiniExcelLibs;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Shell.Monitor.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private string _searchContent;

        public string SearchContent
        {
            get => _searchContent;
            set => SetAndNotify(ref _searchContent, value);
        }

        private BindableCollection<string> _searchHistory = new BindableCollection<string>();

        public BindableCollection<string> SearchHistory
        {
            get => _searchHistory;
            set => SetAndNotify(ref _searchHistory, value);
        }

        private bool _showHistoryPopup;

        public bool ShowHistoryPopup
        {
            get => _showHistoryPopup;
            set => SetAndNotify(ref _showHistoryPopup, value);
        }

        private string _selectedItem;

        public string SelectedItem
        {
            get => _selectedItem;
            set => SetAndNotify(ref _selectedItem, value);
        }

        public ICommand SelectHistoryItemCommand { get; set; }

        public ReportViewModel()
        {
            SelectHistoryItemCommand = MinidaoCommand.Create<string>(ExecuteSelectHistoryItem);
        }

        public void ExecuteSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchContent))
            {
                AddToHistory(SearchContent);
                // 实际搜索逻辑...
            }
        }

        private void AddToHistory(string content)
        {
            if (SearchHistory.Contains(content))
                SearchHistory.Remove(content);

            SearchHistory.Insert(0, content);

            // 限制历史记录数量
            if (SearchHistory.Count > 10)
                SearchHistory.RemoveAt(SearchHistory.Count - 1);
        }

        public void TextBoxGotFocus()
        {
            ShowHistoryPopup = SearchHistory.Any();
        }

        public void TextBoxLostFocus()
        {
            // 延迟关闭以避免立即关闭导致无法点击列表项
            Execute.PostToUIThreadAsync(() => ShowHistoryPopup = false);
        }

        public void ExecuteSelectHistoryItem(string item)
        {
            SearchContent = item;
            ShowHistoryPopup = false;
            ExecuteSearch();
        }

        public void GetStaticticInfo()
        {
            IgniteEventHandler.RaiseStatisticUpdate(this, new IgniteShared.Dtos.StaticticDto());
        }
    }
}