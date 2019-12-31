using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Peanut.Web.API
{
    /// <summary>
    /// 校验特性类
    /// 
    /// 目的：
    ///     1.提供特性编程所需特性
    ///     2.获取模型绑定过程过捕获的验证错误并生成响应内容。
    /// 
    /// 使用规范：
    ///      需要该校验的Class和Action打上此特性
    /// </summary>
    public class DTOValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = 
                    actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}