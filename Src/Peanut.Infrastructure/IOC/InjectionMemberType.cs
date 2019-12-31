using System;


namespace Peanut.Infrastructure.IOC
{
    /// <summary>
    /// 注入成员类型枚举
    /// 
    /// 目的：
    ///     1.定义了要注入的成员属于什么类型的成员
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public enum InjectionMemberType
    {
        Constructor,
        Method,
        Property
    }
}
