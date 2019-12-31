using System;
using System.Collections.Generic;


namespace Peanut.Infrastructure.IOC
{
    /// <summary>
    /// 注入成员类
    /// 
    /// 目的：
    ///     1.指定要注入的成员是属于那个类型，并且指定该成员要注入对象
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public class InjectedMember
    {
        public InjectionMemberType MemberType { get; set; }

        public IList<object> InjectionOjects { get; set; }
    }
}
