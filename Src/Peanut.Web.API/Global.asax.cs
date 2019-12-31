using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Peanut.Infrastructure.IOC;
using Peanut.Infrastructure.Log;
using Peanut.Infrastructure.Log.NLog;
using Peanut.Infrastructure.Mapping;
using Peanut.Infrastructure.Mapping.AutoMapper;
using Peanut.Infrastructure.Security.JWT;
using Peanut.Web.API.ServiceLocate;


namespace Peanut.Web.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var iocService = ServiceLocator.GetService<IIOCService>("Unity");

            //注册应用服务
            //var initAppTask = RegisteApplicationServices();

            //注册日志插件
            iocService.RegisterType<ILogService, NLogAdapter>();

            //注册Mapper插件(DTO和模型映射)
            iocService.RegisterType<IMapperService, AutoMapperAdapter>();

            //注册JWT
            iocService.RegisterType<IJWTService, JWTService>();

            //注册其他类型映射在此注册
        }
    }
}
