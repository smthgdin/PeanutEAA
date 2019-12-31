using System;
using System.Web.Http.Filters;

using Peanut.Web.API.ServiceLocate;
using Peanut.Infrastructure.Log;

namespace Peanut.Web.API
{
    /// <summary>
    /// 日志特性类
    /// 
    /// 目的：
    ///     1.提供特性编程所需特性
    ///     2.自定义日志处理
    /// 
    /// 使用规范：
    ///   
    /// </summary>
    public class ExceptionHandleAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //写日志
            var logger = ServiceLocator.GetService<ILogService>("NLog");
            logger.Error(actionExecutedContext.Exception.ToString());

            //如果是Ajax调用
            //if (actionExecutedContext.Request.Headers.GetValues("X-Requested-With").ToString() == "XMLHttpRequest")
            //{

            //}

            base.OnException(actionExecutedContext);
        }
    }
}