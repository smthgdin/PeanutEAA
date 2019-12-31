/*********************************************************************** 
 * 项目名称 :  Fits.Framework.WCF   
 * 项目描述 :      
 * 类 名 称 :  TraceLogDecorator 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2017/5/23 14:18:38 
 * 更新时间 :  2017/5/23 14:18:38  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
//using System.ComponentModel.Composition;
using System.ServiceModel.Dispatcher;

namespace Peanut.Infrastructure.AOP.WCF
{
    /// <summary>
    /// 操作的装设类（记录操作调用痕迹）
    /// 
    /// 目的：
    ///     包装原有的操作调用；在操作调用结束后写调用痕迹日志。
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    internal class InvokeTraceDecorator : IOperationInvoker
    {
        private readonly IOperationInvoker orgninInvoker;   

        //需要组装的点
        //[Import(typeof(ILogAOP))]
        private ILogAOP logger;

        internal InvokeTraceDecorator(IOperationInvoker originInvoker)
        {
            orgninInvoker = originInvoker;
            //LogPartsContainer.GetLogPartsContainer().ComposeParts(this);
        }

        #region 实现接口

        public object[] AllocateInputs()
        {
            return orgninInvoker.AllocateInputs();
        }

        /// <summary>
        /// 同步调用
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
                var rlt = orgninInvoker.Invoke(instance, inputs, out outputParams);

                logger.WriteLog();  //调用成功才记录日志，异常的话不会记录。

                return rlt;
            }
            finally
            {
                //写日志 
                //logger.WriteLog();
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
            finally
            {
                //写日志
                logger.WriteLog();
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
