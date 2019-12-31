/*********************************************************************** 
 * 项目名称 :  Fits.Framework.Cache   
 * 项目描述 :      
 * 类 名 称 :  RuntimeCacheManagerFactory 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2014/11/21 10:26:41 
 * 更新时间 :  2014/11/21 10:26:41  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System.Collections.Specialized;


namespace Peanut.Infrastructure.Cache.RuntimeCache
{
    /// <summary>
    /// RuntimeCacheManagerFactory类
    /// 
    /// 目的：
    ///     1.提供一个接口用于获取运行时缓存
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public class RuntimeCacheManagerFactory
    {
        /// <summary>
        /// 获取运行时内存缓存管理器对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ICacheService GetRuntimeCacheManager(string name, NameValueCollection config = null)
        {
            return MemoryCacheService.GetInstance(name, config);
        }
    }
}
