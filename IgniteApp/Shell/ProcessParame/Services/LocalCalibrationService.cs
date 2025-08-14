using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgniteApp.Extensions;
using IgniteApp.Shell.ProcessParame.Models;
using IgniteApp.Shell.ProcessParame.ViewModels;
using IgniteShared.Extensions;
using IgniteShared.Globals.Local;
using IT.Tangdao.Framework.DaoAdmin;
using IT.Tangdao.Framework.DaoEnums;
using MiniExcelLibs;
using Stylet.Xaml;

namespace IgniteApp.Shell.ProcessParame.Services
{
    public class LocalCalibrationService
    {
        private static readonly IDaoLogger Logger = DaoLogger.Get(typeof(LoadCalibrationViewModel));

        public static bool CreateExcel(IEnumerable<MotionCalibrationModel> reportDatas)
        {
            const string methodName = nameof(CreateExcel);
            Logger.WriteLocal($"{methodName}: 开始创建Excel文件...");

            try
            {
                // 1. 动态生成路径（使用Path.Combine确保跨平台兼容性）
                string date = DateTime.Now.ToString("yyMMdd");
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString("00"); // 补零，如"01"
                string day = DateTime.Now.Day.ToString("00");    // 补零，如"01"

                string directoryPath = Path.Combine(
                    IgniteInfoLocation.Framework,
                    year,
                    month,
                    day
                );
                string filePath = Path.Combine(directoryPath, $"{date}.xlsx");

                // 2. 确保目录存在（无竞争条件）
                Directory.CreateDirectory(directoryPath); // 如果目录已存在，不会报错

                // 3. 分Sheet处理逻辑
                if (File.Exists(filePath))
                {
                    Logger.WriteLocal($"{methodName}: 检测到已有文件，追加数据...");
                    AppendToExistingFile(filePath, reportDatas);
                }
                else
                {
                    Logger.WriteLocal($"{methodName}: 首次创建文件...");
                    CreateNewFile(filePath, reportDatas);
                }

                Logger.WriteLocal($"{methodName}: 操作成功完成，文件路径: {filePath}");
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.WriteLocal($"{methodName}: 权限不足，无法创建文件或目录。错误: {ex.Message}");
                return false;
            }
            catch (IOException ex)
            {
                Logger.WriteLocal($"{methodName}: 文件读写错误。错误: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLocal($"{methodName}: 发生未预期的错误。错误: {ex.Message}");
                return false; // 根据需求决定是否重新抛出
            }
        }

        public static List<MotionCalibrationModel> GenerateNewData(int count)
        {
            var random = new Random();
            return Enumerable
                .Range(1, count)
                .Select(i => new MotionCalibrationModel
                {
                    //Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                    //CreateDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    //CreateTime = DateTime.Now.ToString("HH:mm"),
                    //MaxValue = Math.Round(random.NextDouble() * 10 + 5, 2),
                    //MinValue = Math.Round(random.NextDouble() * 5 + 5, 2),
                    //Result = random.Next(0, 2) == 0 ? "Ok" : "Ng"
                })
                .ToList();
        }

        private static void CreateNewFile(string filePath, IEnumerable<MotionCalibrationModel> newData)
        {
            var withIdData = newData.SelectMany((item, index) =>
                            {
                                item.Id = index;
                                return newData;
                            });
            var initialSheets = new Dictionary<string, object>
            {
                ["质量数据"] = withIdData,
                ["Ok报表"] = withIdData.Where(x => x.Result.ToYAndN() == "Y").ToList(),
                ["Ng报表"] = withIdData.Where(x => x.Result.ToYAndN() == "N").ToList(),
                ["统计"] = CalculateStatistics(withIdData)
            };
            string templatePath = Path.Combine(IgniteInfoLocation.Framework, "tempframework.xlsx");

            var TemplateData = new TemplateData() { Data = newData.ToList() };
            //var TemplateData = new TemplateData() { Data = initialSheets };
            MiniExcel.SaveAsByTemplate(filePath, templatePath, TemplateData);
            //   MiniExcel.SaveAs(filePath, initialSheets);
            Logger.WriteLocal("保存文件成功");
        }

        private static void AppendToExistingFile(string filePath, IEnumerable<MotionCalibrationModel> newData)
        {
            // 临时文件路径
            string tempFile = Path.Combine(
                Path.GetDirectoryName(filePath),
                $"temp_{Guid.NewGuid():N}.xlsx");

            try
            {
                List<MotionCalibrationModel> realData = newData.ToList();
                // 读取所有现有数据
                //var existingAll = MiniExcel
                //    .Query<MotionCalibrationModel>(filePath, sheetName: "质量数据")
                //    .ToList();
                //var existingOk = MiniExcel
                //    .Query<MotionCalibrationModel>(filePath, sheetName: "Ok报表")
                //    .ToList();
                //var existingNg = MiniExcel
                //    .Query<MotionCalibrationModel>(filePath, sheetName: "Ng报表")
                //    .ToList();
                var localData = MiniExcel.Query<MotionCalibrationModel>(filePath);
                //// 2. 计算新数据的起始ID（现有最大ID + 1）
                int nextId = localData.Count() > 0 ? localData.Max(x => x.Id) + 1 : 0;
                nextId = localData.Count();
                //// 3. 为新数据分配自增ID
                for (int i = 0; i < realData.Count; i++)
                {
                    realData[i].Id = nextId + i;
                }
                //// 合并数据
                //var updatedSheets = new Dictionary<string, object>
                //{
                //    ["质量数据"] = existingAll.Concat(realData).ToList(),
                //    ["Ok报表"] = existingOk
                //        .Concat(realData.Where(x => x.Result.ToYAndN() == "Y"))
                //        .ToList(),
                //    ["Ng报表"] = existingNg
                //        .Concat(realData.Where(x => x.Result.ToYAndN() == "N"))
                //        .ToList(),
                //    ["统计"] = CalculateStatistics(existingAll.Concat(realData).ToList())
                //};

                // 写入临时文件
                // MiniExcel.SaveAs(tempFile, updatedSheets);

                var updateData = localData.Concat(realData);
                string templatePath = Path.Combine(IgniteInfoLocation.Framework, "tempframework.xlsx");

                var TemplateData = new TemplateData() { Data = updateData.ToList() };
                //var TemplateData = new TemplateData() { Data = updatedSheets };
                MiniExcel.SaveAsByTemplate(tempFile, templatePath, TemplateData);
                // 原子性替换文件
                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            finally
            {
                // 清理临时文件
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        private static object CalculateStatistics(IEnumerable<MotionCalibrationModel> data)
        {
            var list = data.ToList();
            return new[]
            {
                new
                {
                    总数量 = list.Count,
                    OK数量 = list.Count(x => x.Result.ToYAndN() == "Y"),
                    NG数量 = list.Count(x => x.Result.ToYAndN() == "N"),
                    不良率 = $"{list.Count(x => x.Result.ToYAndN() == "N") * 100.0 / Math.Max(1, list.Count):F1}%"
                }
            };
        }

        private class TemplateData
        {
            public List<MotionCalibrationModel> Data { get; set; } = new List<MotionCalibrationModel>();
            //public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        }
    }
}