/*********************************************************************** 
 * 项目名称 :  Fits.Framework.Cache   
 * 项目描述 :      
 * 类 名 称 :  CacheManagerHelper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2014/11/10 15:33:30 
 * 更新时间 :  2014/11/10 15:33:30  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
using System.Collections.Generic;


namespace Peanut.Infrastructure.Cache.MemCached
{
    /// <summary>
    /// CacheManagerFactory类
    /// 
    /// 目的：
    ///     1.外部只能使用工厂类来创建logger实例
    ///     2.用于创建CacheManager实例
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public static class CacheManagerFactory
    {
        private static readonly MultiFileWatcher watcher;
        private static readonly object locker;
        private static readonly string filePath;                                    //EnyimClient自己里面写死读取 app.config文件
        private static readonly Dictionary<string, ICacheService> dicCacheClient; 
        
        private const string DEFAULT_KEY = @"enyim.com/memcached";

        static CacheManagerFactory()
        {
            locker = new object();
            dicCacheClient = new Dictionary<string, ICacheService>();
            filePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            //将配置文件添加到监控中，并将事件处理器注册到事件
            watcher = new MultiFileWatcher();
            watcher.Watch(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            watcher.OnChange += OnWatcherChanged;

        }

        /// <summary>
        /// 重新读取配置文件,servers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWatcherChanged(object sender, EventArgs e)
        {
            lock (locker)
            {
                watcher.StopWatching();

                try
                {
                    foreach (KeyValuePair<string, ICacheService> item in dicCacheClient)
                    {
                        item.Value.Reload(filePath, item.Key);
                    }
                }
                finally
                {
                    watcher.Watch(filePath);
                }
            }
        }

        //2014-12-22 xht注释 
        //public static ICacheManager GetCacheManager()
        //{
        //    return GetCacheManager(DEFAULT_KEY);
        //}

        /// <summary>
        /// 获取Enyim内存缓存管理器对象
        /// </summary>
        /// <param name="sectionName">分布式缓存节点,默认是"enyim.com/memcached"</param>
        /// <returns></returns>
        public static ICacheService GetCacheManager(string sectionName = DEFAULT_KEY)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("sectionName");

            lock (locker)
            {
                ICacheService ic;
                if (dicCacheClient.TryGetValue(sectionName, out ic))
                {
                    if (ic != null)
                        return ic;
                }

                ICacheService newCacheClient = MemcachedService.GetInstance(sectionName);                

                dicCacheClient.Add(sectionName, newCacheClient);
                
                return newCacheClient;
            }
        }
    }
}
