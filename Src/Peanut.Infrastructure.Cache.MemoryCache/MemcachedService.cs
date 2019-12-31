/*********************************************************************** 
 * 项目名称 :  Fits.Framework.Cache   
 * 项目描述 :      
 * 类 名 称 :  CacheManager 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2014/10/24 17:15:07 
 * 更新时间 :  2014/10/24 17:15:07  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
using System.Collections.Generic;
using System.Xml;

using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

using Peanut.Helper.XML;

namespace Peanut.Infrastructure.Cache.MemCached
{
    /// <summary>
    /// CacheManager类
    /// 
    /// 目的：
    ///     1.分布式缓存管理API
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    internal class MemcachedService : ICacheService, IDisposable
    {
        private MemcachedClient memClient;
        private readonly ServerStats serverInfo;

        #region 构造函数

        /// <summary>
        /// 使用配置文件默认配置的情况
        /// </summary>
        private MemcachedService()
        {
            memClient = new MemcachedClient();
            serverInfo = memClient.Stats();

            //服务器节点失效事件
            memClient.NodeFailed += memClient_NodeFailed;
        }

        /// <summary>
        /// 使用自定义节点的情况
        /// </summary>
        /// <param name="sectionName"></param>
        private MemcachedService(string sectionName)
        {
            memClient = new MemcachedClient(sectionName);
            serverInfo = memClient.Stats();

            //服务器节点失效事件，需要一个参数为IMemcachedNode并且无返回值的处理器
            memClient.NodeFailed += memClient_NodeFailed;
        }

        /// <summary>
        /// 服务器节点是否失效
        /// 使用MemcachedNode的ping方法
        /// </summary>
        /// <param name="obj">服务器节点</param>
        private static void memClient_NodeFailed(IMemcachedNode obj)
        {
            obj.Ping();
        }

        #endregion

        #region 创建CacheManager实例

        /// <summary>
        /// 创建CacheManager
        /// 如果App.Config或者Web.Config有配置Enyim的情况
        /// </summary>
        /// <returns></returns>
        internal static MemcachedService GetInstance()
        {
            return new MemcachedService();
        }

        /// <summary>
        /// 创建CacheManager
        /// 如果App.Config或者Web.Config有配置Enyim的情况
        /// </summary>
        /// <param name="sectionName">配置文件中，具体的Section节点路径。比如：enyim.com/memcached</param>
        /// <returns></returns>
        internal static MemcachedService GetInstance(string sectionName)
        {
            return new MemcachedService(sectionName);
        }

        #endregion

        #region 公共属性/方法 ICacheManager接口不包含，所以不能实现多态调用，即对于CacheManager才可以调用，RuntimeCache不行。

        /// <summary>
        /// 获取缓存服务器信息
        /// </summary>
        public ServerStats ServerInfo
        {
            get
            {
                return serverInfo ?? memClient.Stats();
            }
        }

        /// <summary>
        /// 获取缓存服务器信息
        /// </summary>
        /// <returns></returns>
        public ServerStats GetServerInfo()
        {
            return serverInfo ?? memClient.Stats();
        }

        #endregion

        #region 接口实现

        /// <summary>
        /// 重新设置Client属性,暂时只重设servers
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <param name="keyName">SoketPool节点Xpath</param>
        public void Reload(string filePath, string keyName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(filePath);

            if (string.IsNullOrWhiteSpace(keyName))
                throw new ArgumentNullException(keyName);

            //todo:以后再重构
            MemcachedClientConfiguration mcc = new MemcachedClientConfiguration();

            #region 读取配置文件服务器节点信息，用于生成MemcachedClientConfiguration

            XmlHelper xml = new XmlHelper(filePath);
            string fullServerPath = string.Format(@"configuration/{0}/servers", keyName);
            XmlNode serverNode = xml.GetNode(fullServerPath);

            for (int i = 0; i < serverNode.ChildNodes.Count - 1; i++)
            {
                //这里过滤注释项
                if (serverNode.ChildNodes[i].NodeType == XmlNodeType.Element)
                {
                    var xmlAttributeCollection = serverNode.ChildNodes[i].Attributes;

                    if (xmlAttributeCollection == null)
                        continue;

                    string add = xmlAttributeCollection["address"].Value.Trim();
                    string port = xmlAttributeCollection["port"].Value.Trim();

                    mcc.AddServer(add, Convert.ToInt32(port));
                }
            }

            #endregion

            mcc.NodeLocator = typeof(DefaultNodeLocator);
            mcc.KeyTransformer = memClient.ClientConfiguration.CreateKeyTransformer();
            mcc.Transcoder = memClient.ClientConfiguration.CreateTranscoder();

            mcc.SocketPool.MinPoolSize = memClient.ClientConfiguration.SocketPool.MinPoolSize;
            mcc.SocketPool.MaxPoolSize = memClient.ClientConfiguration.SocketPool.MaxPoolSize;
            mcc.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
            mcc.SocketPool.DeadTimeout = new TimeSpan(0, 0, 30);

            memClient.ReSetMemcachedClient(mcc);
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

            return memClient.Store(StoreMode.Add, key, value);
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

            return memClient.Store(StoreMode.Add, key, value, expiresAt);
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

            return memClient.Store(StoreMode.Add, key, value, validFor);
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

            return memClient.Store(StoreMode.Set, key, value);
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

            return memClient.Store(StoreMode.Set, key, value, expiresAt);
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

            return memClient.Store(StoreMode.Set, key, value, validFor);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return string.IsNullOrWhiteSpace(key) ? null : memClient.Get(key);
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

            return memClient.Get<T>(key);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, object> Get(string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return null;

            return memClient.Get(keys);
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

            return memClient.Remove(key);
        }

        /// <summary>
        /// 判断是否存在相同Key的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>true：存在相同的Key； false:不存在相同的Key</returns>
        public bool IsExists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            return memClient.Get(key) != null;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <returns></returns>
        public void FlushAll()
        {
            memClient.FlushAll();
        }

        public void Dispose()
        {
            if (memClient == null)
                return;

            memClient.Dispose();
            memClient = null;
        }

        /// <summary>
        /// 清除分区缓存
        /// 只用于运行时内存缓存
        /// </summary>
        /// <param name="regionName">分区名字</param>
        /// <returns>返回值 true：成功；false：失败</returns>
        public bool ClearRegion(string regionName)
        {
            return true;
        }

        #endregion

        #region 新的Enyim客户端不包含API 2014-11-18

        //public bool Add(string key, byte[] value, int offset, int length)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Add, key, value, offset, length);
        //}

        //public bool Add(string key, byte[] value, int offset, int length, DateTime expiresAt)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Add, key, value, offset, length, expiresAt);
        //}

        //public bool Add(string key, byte[] value, int offset, int length, TimeSpan validFor)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Add, key, value, offset, length, validFor);
        //}

        //public bool Set(string key, object value, ulong cas)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value);//.CheckAndSet(key, value, cas);
        //}

        //public bool Set(string key, object value, ulong cas, DateTime expiresAt)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value, expiresAt);
        //}

        //public bool Set(string key, object value, ulong cas, TimeSpan validFor)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value, validFor);
        //}

        //public bool Set(string key, byte[] value, int offset, int length, ulong cas)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.CheckAndSet(key, value, offset, length, cas);
        //}

        //public bool Set(string key, byte[] value, int offset, int length, ulong cas, DateTime expiresAt)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.CheckAndSet(key, value, offset, length, cas, expiresAt);
        //}

        //public bool Set(string key, byte[] value, int offset, int length, ulong cas, TimeSpan validFor)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.CheckAndSet(key, value, offset, length, cas, validFor);
        //}

        //public bool Set(string key, byte[] value, int offset, int length)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value, offset, length);
        //}

        //public bool Set(string key, byte[] value, int offset, int length, DateTime expiresAt)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value, offset, length, expiresAt);
        //}

        //public bool Set(string key, byte[] value, int offset, int length, TimeSpan validFor)
        //{
        //    if (string.IsNullOrWhiteSpace(key))
        //        throw new ArgumentNullException("key");

        //    if (value == null)
        //        throw new ArgumentNullException("value");

        //    return memClient.Store(StoreMode.Set, key, value, offset, length, validFor);
        //}

        //public IDictionary<string, object> Get(string[] keys, out IDictionary<string, ulong> casValues)
        //{
        //    if (keys == null || keys.Length == 0)
        //        throw new ArgumentNullException("keys");

        //    return memClient.Get(keys, out casValues);
        //}

        #endregion

    }
}
