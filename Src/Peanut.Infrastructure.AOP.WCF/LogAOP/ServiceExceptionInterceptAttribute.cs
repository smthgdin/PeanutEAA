using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;


namespace Peanut.Infrastructure.AOP.WCF
{
    /// <summary>
    /// 服务的异常拦截
    /// 
    /// 目的：
    ///     拦截服务使用过程中出现的异常，将异常写入日志。
    /// 
    /// 使用规范：
    ///     在协议中有需要记录异常日志的服务上加上这个特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceExceptionInterceptAttribute : Attribute, IServiceBehavior
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //用于检查服务宿主和服务说明，从而确定服务是否可成功运行。
        }

        /// <summary>
        /// 添加绑定参数
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //用于向绑定元素传递自定义数据。
        }

        /// <summary>
        /// 添加自定义扩展对象
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //循环服务下的终结点
            foreach (var serviceEndpoint in serviceDescription.Endpoints)
            {
                //循环终结点下的操作
                foreach (var operationDescription in serviceEndpoint.Contract.Operations)
                {
                    //如果操作没有附加异常拦截行为则加上
                    if (!operationDescription.Behaviors.OfType<OperationExceptionInterceptAttribute>().Any())
                    {
                        operationDescription.Behaviors.Add(new OperationExceptionInterceptAttribute());
                    }
                }
            }
        }

    }
}
