/*********************************************************************** 
 * 项目名称 :  Fits.Framework.Cache   
 * 项目描述 :      
 * 类 名 称 :  RuntimeCacheManager 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2014/11/21 9:37:51 
 * 更新时间 :  2014/11/21 9:37:51  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;


namespace Peanut.Infrastructure.Cache.RuntimeCache
{
    /// <summary>
    /// RuntimeCacheManager类
    /// 
    /// 目的：
    ///     1.运行时内存缓存，不能使用分布式缓存或者分布式缓存出现问题，可以切换到运行时缓存
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    internal class MemoryCacheService : ICacheService
    {
        private readonly MemoryCache client;

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        private MemoryCacheService(string name, NameValueCollection config)
        {
            //MemoryCache构造函数会对参数进行校验
            client = new MemoryCache(name, config);
        }

        #endregion

        #region 创建CacheManager实例

        /// <summary>
        /// 创建RuntimeCacheManager
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static MemoryCacheService GetInstance(string name, NameValueCollection config)
        {
            return new MemoryCacheService(name, config);
        }

        #endregion

        #region 接口方法

        /// <summary>
        /// 重新加载配置信息
        /// 该方法内存缓存无效
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <param name="keyName">SoketPool节点Xpath</param>
        public void Reload(string filePath, string keyName)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Add(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            return client.Add(key, value, CreatePolicy(null, null));
            //return client.Add(key, value, DateTimeOffset.MaxValue);
        }

        /// <summary>
        /// 添加缓存
        /// 带过期时间
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="expiresAt">过期日期，即到此日期过期</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Add(string key, object value, DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            return client.Add(key, value, CreatePolicy(null, expiresAt));
        }

        /// <summary>
        /// 添加缓存
        /// 带过期时间
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="validFor">有效期，即有效期至该时间</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Add(string key, object value, TimeSpan validFor)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            return client.Add(key, value, CreatePolicy(validFor, null));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Set(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            client.Set(key, value, CreatePolicy(null, null));

            return client.Get(key) == value;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="expiresAt">过期日期，即到此日期过期</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Set(string key, object value, DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            client.Set(key, value, CreatePolicy(null, expiresAt));

            return client.Get(key) == value;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="validFor">有效期，即有效期至该时间</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        public bool Set(string key, object value, TimeSpan validFor)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            if (value == null)
                return false;

            client.Set(key, value, CreatePolicy(validFor, null));

            return client.Get(key) == value;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            return client.Get(key);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            object value = client.Get(key);
            return value is T ? (T)value : default(T);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, object> Get(string[] keys)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>true:删除成功； false:删除失败</returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            return client.Remove(key) != null;
        }

        /// <summary>
        /// 判断是否存在相同Key的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>true：存在相同的Key； false:不存在相同的Key</returns>
        public bool IsExists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(key);

            return client.Contains(key);
        }

        /// <summary>
        /// 清空缓存
        /// 从缓存对象中移除百分比的缓存项 100等于100% 即全部移除
        /// </summary>
        /// <returns></returns>
        public void FlushAll()
        {
            client.Trim(100);
        }

        /// <summary>
        /// 清除分区内的所有缓存
        /// 格式：分区字符串.缓存Key
        /// 意味着在新增缓存的时候，如果需要分区，则在Key前拼接 分区字符串.
        /// </summary>
        /// <returns>返回值 true：成功；false：失败</returns>
        public bool ClearRegion(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
                throw new ArgumentException("regionName is empty");

            foreach (string str in from item in client
                                   where item.Key.StartsWith(regionName + "_")
                                   select item.Key)
            {
                client.Remove(str);
            }

            return true;
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 设置过期策略
        /// </summary>
        /// <param name="slidingExpiration">滑动过期时间</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <returns>缓存项策略</returns>
        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
                policy.AbsoluteExpiration = absoluteExpiration.Value;

            if (slidingExpiration.HasValue)
                policy.SlidingExpiration = slidingExpiration.Value;

            if (!absoluteExpiration.HasValue && !slidingExpiration.HasValue)
                policy.Priority = CacheItemPriority.Default;

            return policy;
        }

        #endregion
    }
}
