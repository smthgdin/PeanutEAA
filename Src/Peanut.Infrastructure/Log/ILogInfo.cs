using System;


namespace Peanut.Infrastructure.Log
{
    /// <summary>
    /// 日志信息接口
    /// 
    /// 目的：
    ///     1.定义了自定义日志信息所包含的属性。
    /// 
    /// 使用规范：
    ///     1.一般多用于将自定义的日志信息用于存入数据库。
    /// </summary>
    public interface ILogInfo
    {
        /// <summary>
        /// 应用程序名
        /// </summary>
        string AppName { get; set; }

        /// <summary>
        /// 程序及名称
        /// </summary>
        string AssemblyName { get; set; }

        /// <summary>
        /// 日志标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        DateTime LogTime { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; set; }
    }
}
