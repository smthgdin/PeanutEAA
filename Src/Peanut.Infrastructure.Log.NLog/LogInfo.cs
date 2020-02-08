using System;

using NL = NLog;

namespace Peanut.Infrastructure.Log.NLog
{
    /// <summary>
    /// NLog下日志信息实现类
    /// 
    /// 目的：
    ///     
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public class NLogInfo : BaseInfo, ILogInfo
    {
        string appName;
        string actionName;
        string className;
        string title;
        LogLevel level;
        string message;
        string logStackTrace;
        DateTime logTime;

        public NLogInfo()
        {
            LogEvent = new NL.LogEventInfo();
        }

        #region 接口实现

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string AppName
        {
            get
            {
                return appName;
            }

            set
            {
                appName = value;
                SetBaseTypeProperties("appName", value);
            }
        }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName
        {
            get
            {
                return actionName;
            }

            set
            {
                actionName = value;
                SetBaseTypeProperties("actionName", value);
            }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get
            {
                return className;
            }

            set
            {
                className = value;
                SetBaseTypeProperties("className", value);
            }
        }

        /// <summary>
        /// 日志信息头
        /// 如果是异常，对应异常的message属性
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                SetBaseTypeProperties("logTitle", value);
            }
        }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;

                NL.LogLevel nlogLevel;

                switch (value)
                {
                    case LogLevel.Debug:
                        nlogLevel = NL.LogLevel.Debug;
                        break;
                    case LogLevel.Error:
                        nlogLevel = NL.LogLevel.Error;
                        break;
                    case LogLevel.Fatal:
                        nlogLevel = NL.LogLevel.Fatal;
                        break;
                    case LogLevel.Info:
                        nlogLevel = NL.LogLevel.Info;
                        break;
                    case LogLevel.Trace:
                        nlogLevel = NL.LogLevel.Trace;
                        break;
                    case LogLevel.Warn:
                        nlogLevel = NL.LogLevel.Warn;
                        break;
                    default:
                        nlogLevel = NL.LogLevel.Error;
                        break;
                }

                LogEvent.Level = nlogLevel;

                SetBaseTypeProperties("logLevel", nlogLevel);
            }
        }

        /// <summary>
        /// 日志信息
        /// 如果是异常，对应异常的StackTrace属性
        /// 不使用基类StackTrace, 因为基类的堆栈信息还不够精确，可能不是异常点
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
                LogEvent.Message = Title + ":" + value;

                SetBaseTypeProperties("logMessage", value);
            }
        }

        /// <summary>
        /// 日志堆栈信息
        /// 异常时的堆栈信息，它是一个或多个堆栈帧的有序集合，即整个调用过程
        /// </summary>
        public string LogStackTrace
        {
            get
            {
                return logStackTrace;
            }

            set
            {
                logStackTrace = value;
                LogEvent.Message = value;

                SetBaseTypeProperties("logStackTrace", value);
            }
        }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime LogTime
        {
            get
            {
                return logTime;
            }

            set
            {
                logTime = value;
                LogEvent.TimeStamp = value;

                SetBaseTypeProperties("logTime", value);
            }
        }

        string userId;

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;

                SetBaseTypeProperties("userId", value);
            }
        }

        string assemblyName;

        public string AssemblyName
        {
            get
            {
                return assemblyName;
            }

            set
            {
                assemblyName = value;

                SetBaseTypeProperties("assemblyName", value);
            }
        }

        #endregion
    }
}
