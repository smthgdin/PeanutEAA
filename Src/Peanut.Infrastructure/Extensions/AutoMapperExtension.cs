using System;
using System.Collections;
using System.Collections.Generic;

using AutoMapper;

namespace Peanut.Infrastructure.Extension.AutoMapper
{
    /// <summary>
    /// AutoMapper扩展类
    /// 
    /// 目的：
    ///     1.实现对象互转。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 扩展AutoMapper
        /// 源对象.MapTo<目标类型>() 得到目标对象实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object sourceObject)
        {
            if (sourceObject == null)
                return default(T);

            var config = new MapperConfiguration(cfg => cfg.CreateMap(sourceObject.GetType(), typeof(T)));
            var mapper = config.CreateMapper();

            return Mapper.Map<T>(sourceObject);
        }

        /// <summary>
        /// 扩展AutoMapper
        /// 源对象.MapTo<List<目标类型>>() 得到目标对象实例列表。
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static List<TDestination> MapTo<TDestination>(this IEnumerable sourceObject)
        {
            if (sourceObject == null)
                return default(List<TDestination>);

            var sourceType = sourceObject.GetType();
            var config = new MapperConfiguration(cfg => cfg.CreateMap(sourceType, typeof(TDestination)));
            var mapper = config.CreateMapper();

            return mapper.Map<List<TDestination>>(sourceObject);
        }

        /// <summary>
        /// 扩展AutoMapper
        /// 针对IEnumerable<TSource>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();

            return mapper.Map<List<TDestination>>(source);
        }
    }
}
