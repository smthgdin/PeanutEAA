/*********************************************************************** 
 * 项目名称 :  Fits.Framework.WCF   
 * 项目描述 :      
 * 类 名 称 :  InvokeWrapper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2017/5/23 11:59:19 
 * 更新时间 :  2017/5/23 11:59:19  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
//using System.ComponentModel.Composition;
using System.ServiceModel.Dispatcher;


namespace Peanut.Infrastructure.AOP.WCF
{
    /// <summary>
    /// 操作的装设类（记录异常）
    /// 
    /// 目的：
    ///     包装原有的操作调用；将原有操作至于try-catch块中以捕捉异常并记录日志。
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    internal class InvokeExceptionDecorator : IOperationInvoker
    {
        private readonly IOperationInvoker orgninInvoker;

        //需要组装的点
        //[Import(typeof(ILogAOP))]
        private ILogAOP logger;

        internal InvokeExceptionDecorator(IOperationInvoker originInvoker)
        {
            orgninInvoker = originInvoker;
            //LogPartsContainer.GetLogPartsContainer().ComposeParts(this);   
        }

        #region 接口实现

        public virtual object[] AllocateInputs()
        {
            return orgninInvoker.AllocateInputs();
        }

        /// <summary>
        /// 同步调用
        /// 在原始调用外面加上Try-Catch块
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <returns></returns>
        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            object[] outputParams = {};

            try
            {
                return orgninInvoker.Invoke(instance, inputs, out outputParams);
            }
            catch(Exception ex)
            {
                //写日志 因为考虑到NLog框架及各项目自身情况，不在本框架里实现写日志（这样程序的灵活性和扩展性高），框架只实现该实现的，其他由具体项目实现
                logger.WriteLog(ex);

                //return null;
                throw;
            }
            finally
            {
                outputs = outputParams;
            }
        }

        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="inputs"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return orgninInvoker.InvokeBegin(instance, inputs, callback, state); 
        }

        /// <summary>
        /// 异步调用结束后操作
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="outputs"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            object[] outputParams = { };

            try
            {
                return orgninInvoker.InvokeEnd(instance, out outputParams, result);
            }
            catch (Exception ex)
            {
                //写日志 因为考虑到NLog框架及各项目自身情况，不在本框架里实现写日志（这样程序的灵活性和扩展性高），框架只实现该实现的，其他由具体项目实现
                logger.WriteLog(ex);

                //return null;
                throw;
            }
            finally
            {
                outputs = outputParams;
            }
        }

        /// <summary>
        /// 获取同步/异步调度信息
        /// </summary>
        public bool IsSynchronous
        {
            get
            {
                return orgninInvoker.IsSynchronous;
            }
        }

        #endregion
    }
}
