using System;
using System.Collections.Generic;


namespace Peanut.Infrastructure.IOC
{
    /// <summary>
    /// IOC服务接口
    /// 
    /// 目的：
    ///     1.定义类IOC服务行为。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public interface IIOCService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="injectionMember">注册时，指定哪个成员需要注入，注入什么值</param>
        void RegisterType<TFrom, TTo>(object injectionMember = null) where TTo : TFrom;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="lifetimePolicy"></param>
        /// <param name="injectionMember">注册时，指定哪个成员需要注入，注入什么值</param>
        void RegisterType<TFrom, TTo>(LifetimePolicy lifetimePolicy, object injectionMember = null) where TTo : TFrom;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="alias"></param>
        /// <param name="injectionMember">注册时，指定哪个成员需要注入，注入什么值</param>
        void RegisterType<TFrom, TTo>(string name, object injectionMember = null) where TTo : TFrom;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="alias"></param>
        /// <param name="lifetimePolicy"></param>
        /// <param name="injectionMember">注册时，指定哪个成员需要注入，注入什么值</param>
        void RegisterType<TFrom, TTo>(string name, LifetimePolicy lifetimePolicy, object injectionMember = null) where TTo : TFrom;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lifetimePolicy"></param>
        /// <param name="injectionMember">注册时，指定哪个成员需要注入，注入什么值</param>
        void RegisterType<T>(LifetimePolicy lifetimePolicy = LifetimePolicy.Transient, object injectionMember = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        T Resolve<T>(string name, Dictionary<string, object> parameters = null) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrides"></param>
        /// <returns></returns>
        T Resolve<T>(Dictionary<string, object> parameters = null) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsRegisted<T>(string name);
    }
}
