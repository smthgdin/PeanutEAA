using System;


namespace Peanut.Infrastructure.Log
{
    /// <summary>
    /// 日志服务接口
    /// 
    /// 目的：
    ///     1.定义了日志服务的行为。
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public interface ILogService
    {
        void Info(string message);

        void Info<T>(string message, T argument);

        void Debug(string message);

        void Debug<T>(string message, T argument);

        void Error(string message);

        void Error<T>(string message, T argument);

        void Error(Exception ex);
    }
}
