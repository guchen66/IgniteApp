using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.Collections;
using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Home.Models
{
    public static class DataConfig
    {
        // 三个独立的线程安全字典
        private static readonly TangdaoSortedDictionary<string, string> _svData =
            new TangdaoSortedDictionary<string, string>();

        private static readonly TangdaoSortedDictionary<string, string> _dvData =
            new TangdaoSortedDictionary<string, string>();

        private static readonly TangdaoSortedDictionary<string, string> _cvData =
            new TangdaoSortedDictionary<string, string>();

        // 提供只读访问
        public static IReadOnlyDictionary<string, string> SVData => _svData;

        public static IReadOnlyDictionary<string, string> DVData => _dvData;
        public static IReadOnlyDictionary<string, string> CVData => _cvData;

        public static TangdaoSortedDictionary<string, string> dicts = new TangdaoSortedDictionary<string, string>();

        // 分别初始化
        private static void InitializeSV()
        {
            var keys = LoadKeysFromExcel("SV");
            _svData.Clear();

            foreach (var key in keys)
            {
                _svData.TryAdd(key, string.Empty);
            }
        }

        private static void InitializeDV()
        {
            var keys = LoadKeysFromExcel("DV");
            _dvData.Clear();

            foreach (var key in keys)
            {
                _dvData.TryAdd(key, string.Empty);
            }
        }

        private static void InitializeCV()
        {
            var keys = LoadKeysFromExcel("CV");
            _cvData.Clear();

            foreach (var key in keys)
            {
                _cvData.TryAdd(key, string.Empty);
            }
        }

        public static void InitializeFromExcel()
        {
            InitializeSV();
            //  InitializeDV();
            //  InitializeCV();
        }

        public static void UpdateSVReportValues(List<string> allDatas)
        {
            if (allDatas == null || allDatas.Count != _svData.Count)
            {
                throw new ArgumentException("PLC数据数量与key数量不匹配");
            }

            //   dicts.UpdateValues(allDatas);
            if (allDatas.Count == _svData.Count)
            {
                // 使用 Zip 组合，然后逐个更新
                var updates = _svData.Keys.Zip(allDatas, (key, value) => new { Key = key, Value = value });

                foreach (var item in updates.ToList())
                {
                    _svData.AddOrUpdate(item.Key, item.Value, (k, oldValue) => item.Value);
                }
            }
            else
            {
                Console.WriteLine("List和Dictionary的大小不一致");
                return;
            }
        }

        public static void UpdateDVReportValues(List<string> allDatas)
        {
            if (allDatas == null || allDatas.Count != _dvData.Count)
            {
                throw new ArgumentException("PLC数据数量与key数量不匹配");
            }
            if (allDatas.Count == _dvData.Count)
            {
                // 使用 Zip 组合，然后逐个更新
                var updates = _dvData.Keys.Zip(allDatas, (key, value) => new { Key = key, Value = value });

                foreach (var item in updates)
                {
                    _dvData.AddOrUpdate(item.Key, item.Value, (k, oldValue) => item.Value);
                }
            }
            else
            {
                Console.WriteLine("List和Dictionary的大小不一致");
                return;
            }
        }

        public static void UpdateCVReportValues(List<string> allDatas)
        {
            if (allDatas == null || allDatas.Count != _cvData.Count)
            {
                throw new ArgumentException("PLC数据数量与key数量不匹配");
            }

            if (allDatas.Count == _cvData.Count)
            {
                // 使用 Zip 组合，然后逐个更新
                var updates = _cvData.Keys.Zip(allDatas, (key, value) => new { Key = key, Value = value });

                foreach (var item in updates)
                {
                    _cvData.AddOrUpdate(item.Key, item.Value, (k, oldValue) => item.Value);
                }
            }
            else
            {
                Console.WriteLine("List和Dictionary的大小不一致");
                return;
            }
        }

        private static IEnumerable<string> LoadKeysFromExcel(string reportName)
        {
            var excelPath = Path.Combine(IgniteInfoLocation.Framework, $"{reportName}Report.xlsx");
            var lists = MiniExcelLibs.MiniExcel.Query<ReportData>(excelPath);
            // 只返回Name列，并去重和过滤空值
            return lists
                .Select(x => x.Name)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct();
        }
    }
}