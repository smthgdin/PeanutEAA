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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="argument"></param>
        void Info<T>(T argument, string message = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="argument"></param>
        void Debug<T>(T argument, string message = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="message"></param>
        void Error<T>(T argument, string message = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);
    }
}
