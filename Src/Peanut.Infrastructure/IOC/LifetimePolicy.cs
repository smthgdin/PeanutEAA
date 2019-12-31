using System;

namespace Peanut.Infrastructure.IOC
{
    /// <summary>
    /// 对象生命周期枚举
    /// 
    /// 目的：
    ///     1.定义对象生命周期策略
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public enum LifetimePolicy
    {
        Transient,
        PerThread,
        Singleton
    }
}
