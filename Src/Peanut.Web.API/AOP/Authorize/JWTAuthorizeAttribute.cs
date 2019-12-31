using System.Web.Http;
using System.Web.Http.Controllers;

using Peanut.Infrastructure.Security.JWT;
using Peanut.Web.API.ServiceLocate;

namespace Peanut.Web.API
{
    /// <summary>
    /// 认证Token特性类
    /// 
    /// 目的：
    ///     1.提供特性编程所需特性
    ///     2.自定义认证Token的校验
    /// 
    /// 使用规范：
    ///     需要该认证的Class和Action打上此特性
    /// </summary>
    public class JWTAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //var tokenString = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            if (actionContext.Request.Headers.Authorization != null && actionContext.Request.Headers.Authorization.Parameter != null)
            {
                //客户端可将Token放在请求头或者放在post请求的数据体中，为了满足所有http操作所以把Token放在header。
                var tokenString = actionContext.Request.Headers.Authorization.Parameter;
                var jwtService = ServiceLocator.GetService<JWTService>("JWT");
                var rlt = jwtService.ValidateToken(tokenString);
                return rlt.Item1 == 1 ? true : false;
            }

            return false;
        }
    }
}