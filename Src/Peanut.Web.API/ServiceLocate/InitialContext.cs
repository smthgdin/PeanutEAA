using System;
using System.Collections.Generic;

using Peanut.Infrastructure.IOC;
using Peanut.Infrastructure.Log;
using Peanut.Infrastructure.Security.JWT;
using Peanut.Infrastructure.Mapping;


namespace Peanut.Web.API.ServiceLocate
{
    /// <summary>
    /// 初始化上下文类
    /// 
    /// 目的：
    ///     1.创建服务对象
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    internal class InitialContext
    {
        private readonly IIOCService ioc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocService">注入ioc服务</param>
        internal InitialContext(IIOCService iocService)
        {
            ioc = iocService;
        }

        /// <summary>
        /// 服务查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LookupSerive<T>(string name) where T : class  
        {
            //todo:待优化
            if (name.Equals("NLog"))
            {
                var dict = new Dictionary<string, object>();
                dict.Add("loggerName", "TxtLog");

                return (T)ioc.Resolve<ILogService>(dict);
            }

            if (name.Equals("AutoMapper"))
            {
                return (T)ioc.Resolve<IMapperService>();
            }

            if (name.Equals("JWT"))
            {
                return ioc.Resolve<IJWTService>() as T;
            }

            return default(T);
        }
    }
}