using System;
using System.Collections.Generic;

using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

using Peanut.Infrastructure.IOC;

namespace Peanut.Infrastructure.IOC.Unity
{
    /// <summary>
    /// Unity容器适配类
    /// 
    /// 目的：
    ///     1.将unity容器适配进系统。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public class UnityIOCAdapter : IIOCService
    {
        private readonly IUnityContainer container;
        //如果容器不做单例，会有一些问题。
        //private static readonly Lazy<U.IUnityContainer> container = new Lazy<U.IUnityContainer>(()=> new U.UnityContainer());

        public UnityIOCAdapter()
        {
            container = new UnityContainer();
        }

        public UnityIOCAdapter(IUnityContainer unityContainer)
        {
            container = unityContainer;
        }

        public void RegisterType<TFrom, TTo>(object injectionMembers = null) where TTo : TFrom
        {
            InjectionMember[] members = null;

            MapInjectionMember(ref members, injectionMembers);
            
            container.RegisterType(typeof(TFrom),typeof(TTo), "", new TransientLifetimeManager(), members);
        }

        public void RegisterType<TFrom, TTo>(LifetimePolicy lifetimePolicy, object injectionMembers = null) where TTo : TFrom
        {
            InjectionMember[] members = null;

            MapInjectionMember(ref members, injectionMembers);

            if (lifetimePolicy == LifetimePolicy.PerThread)
                container.RegisterType(typeof(TFrom), typeof(TTo), "", new TransientLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Singleton)
                container.RegisterType(typeof(TFrom), typeof(TTo), "", new ContainerControlledLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Transient)
                container.RegisterType(typeof(TFrom), typeof(TTo), "", new PerThreadLifetimeManager(), members);
           
        }

        public void RegisterType<TFrom, TTo>(string name, object injectionMembers = null) where TTo : TFrom
        {
            InjectionMember[] members = null;

            MapInjectionMember(ref members, injectionMembers);

            container.RegisterType(typeof(TFrom), typeof(TTo), name, new TransientLifetimeManager(), members);
        }

        public void RegisterType<TFrom, TTo>(string name, LifetimePolicy lifetimePolicy, object injectionMembers = null) where TTo : TFrom
        {
            InjectionMember[] members = null;

            MapInjectionMember(ref members, injectionMembers);

            if (lifetimePolicy == LifetimePolicy.PerThread)
                container.RegisterType(typeof(TFrom), typeof(TTo), name, new TransientLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Singleton)
                container.RegisterType(typeof(TFrom), typeof(TTo), name, new ContainerControlledLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Transient)
                container.RegisterType(typeof(TFrom), typeof(TTo), name, new PerThreadLifetimeManager(), members);
        }

        public void RegisterType<T>(LifetimePolicy lifetimePolicy, object injectionMembers = null)
        {
            InjectionMember[] members = null;

            MapInjectionMember(ref members, injectionMembers);

            if (lifetimePolicy == LifetimePolicy.PerThread)
                container.RegisterType(typeof(T), typeof(T), string.Empty, new TransientLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Singleton)
                container.RegisterType(typeof(T), typeof(T), string.Empty, new ContainerControlledLifetimeManager(), members);

            if (lifetimePolicy == LifetimePolicy.Transient)
                container.RegisterType(typeof(T), typeof(T), string.Empty, new PerThreadLifetimeManager(), members);
        }

        public T Resolve<T>(string name, Dictionary<string, object> parameters = null) where T : class
        {
            if(parameters == null || parameters.Count == 0)
                return (T)container.Resolve(typeof(T), name, null);
            else
            {
                var parameterOverrides = new ResolverOverride[parameters.Count];

                int i = 0;
                foreach (var param in parameters)
                {
                    parameterOverrides[i] = new ParameterOverride(param.Key, param.Value);
                    i++;
                }

                return (T)container.Resolve(typeof(T), name, parameterOverrides);
            }
            
        }

        public T Resolve<T>(Dictionary<string, object> parameters = null) where T : class
        {
            if (parameters == null || parameters.Count == 0)
                return (T)container.Resolve(typeof(T), string.Empty, null);
            else
            {
                var parameterOverrides = new ResolverOverride[parameters.Count];

                int i = 0;
                foreach (var param in parameters)
                {
                    parameterOverrides[i] = new ParameterOverride(param.Key, param.Value);
                    i++;
                }

                return (T)container.Resolve(typeof(T), string.Empty, parameterOverrides);
            }
           
        }

        public bool IsRegisted<T>(string name)
        {
            return container.IsRegistered(typeof(T), name);
        }

        #region Private Method

        private void MapInjectionMember(ref InjectionMember[] injectionMembers, object injectionObjects)
        {
            var tmp = injectionObjects as InjectedMember;

            if (tmp == null || tmp.InjectionOjects.Count == 0)
                return;
            else
                injectionMembers = new InjectionMember[tmp.InjectionOjects.Count];

            if (tmp.MemberType == InjectionMemberType.Constructor)
            {
                for(int i = 0; i< tmp.InjectionOjects.Count; i++)
                {
                    var c = new InjectionConstructor(tmp.InjectionOjects[0]);
                    injectionMembers[i] = c;
                }               
            }

            if (tmp.MemberType == InjectionMemberType.Method)
            {
                for (int i = 0; i < tmp.InjectionOjects.Count; i++)
                {
                    injectionMembers[i] = tmp.InjectionOjects[0] as InjectionMember;
                }
            }

            if (tmp.MemberType == InjectionMemberType.Property)
            {
                for (int i = 0; i < tmp.InjectionOjects.Count; i++)
                {
                    injectionMembers[i] = tmp.InjectionOjects[0] as InjectionMember;
                }
            }

            //foreach (var obj in injectionObjects)
            //{
            //    InjectionMember member = default(InjectionMember);

            //    if (obj is InjectionConstructor)
            //    {
            //        var tmp = obj as InjectionConstructor;
            //        if (tmp == null)
            //            throw new Exception("对象转InjectionConstructor出错");

            //        member = new InjectionConstructor(tmp.ArgValues);
            //    }

            //    if (obj is InjectionProperty)
            //    {
            //        var tmp = obj as InjectionProperty;


            //        if (tmp.Value == null)
            //            member = new Microsoft.Practices.Unity.InjectionProperty(tmp.Name);
            //        else
            //            member = new Microsoft.Practices.Unity.InjectionProperty(tmp.Name, tmp.Value);
            //    }

            //    if (obj is InjectionMethod)
            //    {
            //        var tmp = obj as InjectionMethod;
            //        member = new Microsoft.Practices.Unity.InjectionMethod(tmp.Name, tmp.ArgValues);
            //    }

            //    injectionMembers[i] = member;
            //}
        }

        #endregion
    }
}
