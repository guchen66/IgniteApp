using IgniteDevices.Core.Models.Results;
using IT.Tangdao.Framework.Abstractions.IServices;
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
        QueryableResult<T> SelectSingle(string recipe);

        QueryableResult<List<T>> SelectList(string recipe);

        IQueryableResult Save(T entity);

        IQueryableResult SaveList(List<T> entities);
    }

    public class XmlReadProvider<T> : IReadProvider<T> where T : new()
    {
        private readonly IReadService _readService;
        private readonly string _fileName;

        public XmlReadProvider(IReadService readService, string fileName = null)
        {
            _readService = readService;

            _fileName = fileName ?? $"{typeof(T).Name}.xml";
        }

        public QueryableResult<T> SelectSingle(string recipe)
        {
            try
            {
                var filePath = Path.Combine(recipe, _fileName);
                var xmlData = _readService.Read(filePath);

                if (xmlData == null)
                    return QueryableResult<T>.Failure("XML数据为空或文件不存在");

                _readService.Current.Load(xmlData);
                var result = _readService.Current.SelectNodes<T>();

                if (result.IsSuccess && result.Result != null && result.Result.Any())
                    return QueryableResult<T>.Success(result.Data.First());
                else
                    return QueryableResult<T>.Failure("未找到数据或读取失败");
            }
            catch (Exception ex)
            {
                return QueryableResult<T>.Failure($"读取单条数据失败: {ex.Message}", ex);
            }
        }

        public QueryableResult<List<T>> SelectList(string recipe)
        {
            try
            {
                var filePath = Path.Combine(recipe, _fileName);
                var xmlData = _readService.Read(filePath);

                if (xmlData == null)
                    return QueryableResult<List<T>>.Failure("XML数据为空或文件不存在");

                _readService.Current.Load(xmlData);
                var result = _readService.Current.SelectNodes<T>();

                if (result.IsSuccess)
                    return QueryableResult<List<T>>.Success(result.Data ?? new List<T>());
                else
                    return QueryableResult<List<T>>.Failure("读取列表数据失败");
            }
            catch (Exception ex)
            {
                return QueryableResult<List<T>>.Failure($"读取列表数据失败: {ex.Message}", ex);
            }
        }

        public IQueryableResult Save(T entity)
        {
            // 实现保存单条数据的逻辑
            throw new NotImplementedException();
        }

        public IQueryableResult SaveList(List<T> entities)
        {
            // 实现保存列表数据的逻辑
            throw new NotImplementedException();
        }
    }
}