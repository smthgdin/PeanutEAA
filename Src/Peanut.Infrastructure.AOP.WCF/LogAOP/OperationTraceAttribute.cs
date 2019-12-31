/*********************************************************************** 
 * 项目名称 :  Fits.Framework.WCF   
 * 项目描述 :      
 * 类 名 称 :  OperationTraceAttribute 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2017/5/23 13:03:46 
 * 更新时间 :  2017/5/23 13:03:46  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Peanut.Infrastructure.AOP.WCF
{
    /// <summary>
    /// 操作的痕迹跟踪
    /// 
    /// 目的：
    ///     拦截操作调用，在调用之后将调用信息写入日志。
    /// 
    /// 使用规范：
    ///     在协议中有需要记录调用痕迹的操作上加上这个特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OperationTraceAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
            //用于检查服务宿主和服务说明，从而确定服务是否可成功运行。
        }

        /// <summary>
        /// 添加绑定参数
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            //用于向绑定元素传递自定义数据。
        }

        /// <summary>
        /// 添加自定义扩展对象（服务端使用）
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="clientOperation"></param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            //客户端附加的操作行为，服务端不用管。
        }

        /// <summary>
        /// 添加自定义扩展对象
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            IOperationInvoker originInvoker = dispatchOperation.Invoker;
            dispatchOperation.Invoker = new InvokeTraceDecorator(originInvoker);
        }      
    }
}
