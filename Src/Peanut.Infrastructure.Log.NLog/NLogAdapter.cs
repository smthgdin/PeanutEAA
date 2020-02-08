using System;

using NL = NLog;

using NLog;

namespace Peanut.Infrastructure.Log.NLog
{
    /// <summary>
    /// NLog适配器类
    /// 
    /// 目的：
    ///     1.将NLog适配进系统。
    ///     2.实现了日志服务接口的行为。
    /// 使用规范：
    ///     
    /// </summary>
    public class NLogAdapter : ILogService
    {
        private readonly Logger logger;

        public NLogAdapter(string loggerName)
        {
            logger = LogManager.GetLogger(loggerName);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug<T>(T argument, string message = "")
        {
            logger.Debug(message, argument);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        //需配合配置文件设置，目前实现是写入数据库，而且是不使用nlog预设属性的。
        public void Error(Exception ex)
        {
            logger.ErrorException(ex.Message, ex);
        }

        public void Error<T>(T argument, string message = "")
        {
            LogEventInfo tmp = new LogEventInfo();

            var properties = argument.GetType().GetProperties();
            foreach (var property in properties)
            {
                var n = property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1);
                tmp.Properties[n] = property.GetValue(argument);
            }

            tmp.Level = NL.LogLevel.Error;

            logger.Log(tmp);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info<T>(T argument, string message = "")
        {
            LogEventInfo tmp = new LogEventInfo();

            var properties = argument.GetType().GetProperties();
            foreach (var property in properties)
            {
                var n = property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1);
                tmp.Properties[n] = property.GetValue(argument);
            }

            tmp.Level = NL.LogLevel.Info;

            logger.Log(tmp);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn<T>(string message, T argument)
        {
            logger.Warn(message, argument);
        }
    }
}
