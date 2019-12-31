using System;

namespace Peanut.Infrastructure.AOP.WCF
{
    /// <summary>
    /// WCF下的AOP接口
    /// </summary>
    public interface ILogAOP
    {
        /// <summary>
        /// 写异常日志
        /// 实现类确定写到哪里和写什么内容
        /// </summary>
        void WriteLog(Exception ex);

        /// <summary>
        /// 写非异常日志
        /// 实现类确定写到哪里和写什么内容
        /// </summary>
        void WriteLog();
    }
}
