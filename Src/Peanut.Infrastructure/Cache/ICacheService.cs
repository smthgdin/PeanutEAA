using System;
using System.Collections.Generic;


namespace Peanut.Infrastructure.Cache
{
    /// <summary>
    /// 缓存接口
    /// 
    /// 目的：
    ///     1.分布式缓存管理API
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 重新加载配置信息
        /// 该方法内存缓存无效
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <param name="keyName">SoketPool节点Xpath</param>
        void Reload(string filePath, string keyName);

        ///// <summary>
        ///// 获取缓存服务器信息
        ///// 该属性内存缓存无效
        ///// </summary>
        //ServerStats ServerInfo { get; }

        ///// <summary>
        ///// 获取缓存服务器信息
        ///// 该方法内存缓存无效
        ///// </summary>
        ///// <returns></returns>
        //ServerStats GetServerInfo();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Add(string key, object value);

        /// <summary>
        /// 添加缓存
        /// 带过期时间
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="expiresAt">过期日期，即到此日期过期</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Add(string key, object value, DateTime expiresAt);

        /// <summary>
        /// 添加缓存
        /// 带过期时间
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="validFor">有效期，即有效期至该时间</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Add(string key, object value, TimeSpan validFor);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Set(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="expiresAt">过期日期，即到此日期过期</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Set(string key, object value, DateTime expiresAt);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">要缓存的对象</param>
        /// <param name="validFor">有效期，即有效期至该时间</param>
        /// <returns>true:添加成功； false:添加失败</returns>
        bool Set(string key, object value, TimeSpan validFor);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        IDictionary<string, object> Get(string[] keys);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>true:删除成功； false:删除失败</returns>
        bool Remove(string key);

        /// <summary>
        /// 判断是否存在相同Key的缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>true：存在相同的Key； false:不存在相同的Key</returns>
        bool IsExists(string key);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void FlushAll();

        /// <summary>
        /// 清除分区内的所有缓存,只用于运行时缓存
        /// 格式：分区字符串.缓存Key
        /// 意味着在新增缓存的时候，如果需要分区，则在Key前拼接 分区字符串.
        /// </summary>
        /// <returns>返回值 true：成功；false：失败</returns>
        bool ClearRegion(string regionName);

        #region 新的Enyim客户端不包含API 2014-11-18

        ///// <summary>
        ///// 添加缓存
        ///// </summary>
        ///// <param name="key">缓存Key</param>
        ///// <param name="value">要缓存的对象</param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <returns>true:添加成功； false:添加失败</returns>
        //bool Add(string key, byte[] value, int offset, int length);

        ///// <summary>
        ///// 添加缓存
        ///// </summary>
        ///// <param name="key">缓存Key</param>
        ///// <param name="value">要缓存的对象</param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="expiresAt"></param>
        ///// <returns>true:添加成功； false:添加失败</returns>
        //bool Add(string key, byte[] value, int offset, int length, DateTime expiresAt);

        ///// <summary>
        ///// 添加缓存
        ///// </summary>
        ///// <param name="key">缓存Key</param>
        ///// <param name="value">要缓存的对象</param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="validFor"></param>
        ///// <returns>true:添加成功； false:添加失败</returns>
        //bool Add(string key, byte[] value, int offset, int length, TimeSpan validFor);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="cas"></param>
        ///// <returns></returns>
        //bool Set(string key, object value, ulong cas);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="cas"></param>
        ///// <param name="expiresAt"></param>
        ///// <returns></returns>
        //bool Set(string key, object value, ulong cas, DateTime expiresAt);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="cas"></param>
        ///// <param name="validFor"></param>
        ///// <returns></returns>
        //bool Set(string key, object value, ulong cas, TimeSpan validFor);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="cas"></param>
        ///// <returns></returns>
        // bool Set(string key, byte[] value, int offset, int length, ulong cas);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="cas"></param>
        ///// <param name="expiresAt"></param>
        ///// <returns></returns>
        //bool Set(string key, byte[] value, int offset, int length, ulong cas, DateTime expiresAt);

        ///// <summary>
        ///// 设置缓存
        ///// 带校验
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="cas"></param>
        ///// <param name="validFor"></param>
        ///// <returns></returns>
        //bool Set(string key, byte[] value, int offset, int length, ulong cas, TimeSpan validFor);

        ///// <summary>
        ///// 获取缓存数据
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <param name="casValues"></param>
        ///// <returns></returns>
        //IDictionary<string, object> Get(string[] keys, out IDictionary<string, ulong> casValues);

        ///// <summary>
        ///// 设置缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //bool Set(string key, byte[] value, int offset, int length);

        ///// <summary>
        ///// 设置缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="expiresAt"></param>
        ///// <returns></returns>
        //bool Set(string key, byte[] value, int offset, int length, DateTime expiresAt);

        ///// <summary>
        ///// 设置缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <param name="offset"></param>
        ///// <param name="length"></param>
        ///// <param name="validFor"></param>
        ///// <returns></returns>
        //bool Set(string key, byte[] value, int offset, int length, TimeSpan validFor);

        #endregion
    }
}
