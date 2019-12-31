using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Debug<T>(string message, T argument)
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
            if (ex == null)
                throw new ArgumentNullException();

            //IUser user = User.CurrentUser;

            //LogInfo info = new LogInfo
            //{
            //    LogLevel = LogLevel.Error,
            //    LogTitle = ex.Message,
            //    LogMessage = ex.StackTrace,
            //    LogStackTrace = new StackTrace().ToString(),
            //    LogTime = DateTime.Now,
            //    UserId = user == null ? string.Empty : user.UserId
            //};

            //logger.Log(info.LogEventInfo);
        }

        public void Error(ILogInfo info)
        {
            LogEventInfo tmp = new LogEventInfo();
            tmp.Properties["appName"] = info.AppName;
            tmp.Properties["assemblyName"] = info.AssemblyName;
            tmp.Properties["logTitle"] = info.Title;
            tmp.Properties["logMessage"] = info.Message;
            tmp.Properties["logTime"] = info.LogTime;
            tmp.Properties["userId"] = info.UserId;
            tmp.Properties["logLevel"] = NL.LogLevel.Error;
            tmp.Level = NL.LogLevel.Error;
            tmp.LoggerName = "SqlLog";

            logger.Log(tmp);
        }

        public void Error<T>(string message, T argument)
        {
            logger.Error(message, argument);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info<T>(string message, T argument)
        {
            logger.Info(message, argument);
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
