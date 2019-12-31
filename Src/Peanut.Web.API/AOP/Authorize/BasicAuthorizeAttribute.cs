using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

using Peanut.Infrastructure.Security.Basic;


namespace Peanut.Web.API
{
    /// <summary>
    /// HTTP Basic认证特性类
    /// 
    /// 目的：
    ///     1.提供特性编程所需特性
    ///     2.自定义校验
    /// 
    /// 使用规范：
    ///     需要该认证的Class和Action打上此特性
    /// </summary>
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写基类方法
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Options)
                return true;

            if (actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Parameter != null)
            {
                var authorizationParameter = Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter);
                var basicArray = Encoding.Default.GetString(authorizationParameter).Split(':');
                var userid = basicArray[0];
                var password = basicArray[1];

                var systemSection = (PeripheralSystemSection)ConfigurationManager.GetSection("peripheralSystemGroup");
                foreach (var peripheralsystem in systemSection.PeripheralSystems)
                {
                    var tmp = ((PeripheralSystem)peripheralsystem);

                    if (tmp.User == userid && tmp.Password == password)
                        return true;
                }
            }

            return false;
        }
    }
}