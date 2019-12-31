using System;


namespace Peanut.Infrastructure.Log
{
    /// <summary>
    /// 日志等级枚举
    /// 
    /// 目的：
    ///     1.定义了日志的等级。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 跟踪
        /// </summary>
        Trace,
        /// <summary>
        /// 调式
        /// </summary>
        Debug,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 致命
        /// </summary>
        Fatal
    }
}
