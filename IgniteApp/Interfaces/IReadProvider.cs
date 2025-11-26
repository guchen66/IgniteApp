using IgniteDevices.Core.Models.Results;
using IT.Tangdao.Framework.Abstractions.FileAccessor;
using IT.Tangdao.Framework.Abstractions.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    /// <summary>
    /// 进行二次封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadProvider<T> where T : new()
    {
        ResponseResult<T> SelectSingle(string recipe);

        ResponseResult<List<T>> SelectList(string recipe);

        IResponseResult Save(T entity);

        IResponseResult SaveList(List<T> entities);
    }

    public class XmlReadProvider<T> : IReadProvider<T> where T : new()
    {
        private readonly IContentReader _readService;
        private readonly string _fileName;

        public XmlReadProvider(IContentReader readService, string fileName = null)
        {
            _readService = readService;

            _fileName = fileName ?? $"{typeof(T).Name}.xml";
        }

        public ResponseResult<T> SelectSingle(string recipe)
        {
            try
            {
                var filePath = Path.Combine(recipe, _fileName);
                var xmlData = _readService.Default.Read(filePath).Content;

                if (xmlData == null)
                    return ResponseResult<T>.Failure("XML数据为空或文件不存在");

                // _readService.Current.Load(xmlData);
                var result = _readService.Default.AsXml().SelectNodes<T>();

                if (result.IsSuccess && result.Data != null)
                    return ResponseResult<T>.Success(result.Data.First());
                else
                    return ResponseResult<T>.Failure("未找到数据或读取失败");
            }
            catch (Exception ex)
            {
                return ResponseResult<T>.Failure($"读取单条数据失败: {ex.Message}", ex);
            }
        }

        public ResponseResult<List<T>> SelectList(string recipe)
        {
            try
            {
                var filePath = Path.Combine(recipe, _fileName);
                var xmlData = _readService.Default.Read(filePath).Content;

                if (xmlData == null)
                    return ResponseResult<List<T>>.Failure("XML数据为空或文件不存在");

                var result = _readService.Default.Read(filePath).AsXml().SelectNodes<T>();

                if (result.IsSuccess)
                    return ResponseResult<List<T>>.Success(result.Data ?? new List<T>());
                else
                    return ResponseResult<List<T>>.Failure("读取列表数据失败");
            }
            catch (Exception ex)
            {
                return ResponseResult<List<T>>.Failure($"读取列表数据失败: {ex.Message}", ex);
            }
        }

        public IResponseResult Save(T entity)
        {
            // 实现保存单条数据的逻辑
            throw new NotImplementedException();
        }

        public IResponseResult SaveList(List<T> entities)
        {
            // 实现保存列表数据的逻辑
            throw new NotImplementedException();
        }
    }
}