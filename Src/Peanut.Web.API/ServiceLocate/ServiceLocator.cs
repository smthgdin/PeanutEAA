using System.Collections.Generic;
using System.Linq;

using Peanut.Infrastructure.IOC.Unity;

namespace Peanut.Web.API.ServiceLocate
{
    /// <summary>
    /// 服务定位器类
    /// 
    /// 目的：
    ///     1.为API应用服务层提供常用的服务
    ///     2.应用服务层只管调用，不需要关注如何创建。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    internal class ServiceLocator
    {
        private static IDictionary<string, dynamic> serviceCache = new Dictionary<string, dynamic>();
        private static readonly object lockObj = new object();

        internal static T GetService<T>(string name) where T : class
        {
            if (!serviceCache.Any(x => x.Key.Equals(name)))
            {
                lock (lockObj)
                {
                    if (!serviceCache.Any(x => x.Key.Equals(name)))
                    {
                        //IOC服务比较特殊，其他服务是基于它来创建的，所以不和其他服务放在InitialContext里创建。
                        //todo：待优化
                        if (name.Equals("Unity"))
                        {
                            dynamic unityService = new UnityIOCAdapter();
                            serviceCache.Add(name, unityService);

                            return (T)unityService;
                        }

                        var iocService = serviceCache.Single(x => x.Key == "Unity").Value;
                        var ctx = new InitialContext(iocService);
                        var service = ctx.LookupSerive<T>(name);
                        if (service != null)
                        {
                            serviceCache.Add(name, service);
                            return service;
                        }
                    }

                }
            }

            return (T)serviceCache.Single(x => x.Key.Equals(name)).Value;
        }

        internal static bool RemoceService<T>(string name) where T : class
        {
            if(serviceCache.Keys.Contains(name))
                return serviceCache.Remove(name);

            return true;
        }
    }
}