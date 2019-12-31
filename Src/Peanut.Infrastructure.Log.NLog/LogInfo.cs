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
    public class NLogInfo : ILogInfo
    {
        /// <summary>
        /// 构造函数
        /// 初始化基类属性Level默认值（默认等级Error），未初始化会抛出“未将对象引用设置到对象的实例。”异常
        /// </summary>
        public NLogInfo()
        {
            //初始化 默认初始化等级为Error,目的是避免配置文件中,路由规则高于默认等级，而代码又没有指定等级，会不写日志
            LogEventInfo = new NL.LogEventInfo { Level = NL.LogLevel.Error };
        }

        #region 属性

        #region 定义LogEventInfo对象，将LogInfo对象的属性转到LogEventInfo特定的属性时用到

        /// <summary>
        /// NLog里的日志事件参数
        /// </summary>
        internal NL.LogEventInfo LogEventInfo { get; set; }

        #endregion

        private string appName;

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

        private string assemblyName;

        /// <summary>
        /// 程序集名称
        /// </summary>
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

        private string className;

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

        private string methodName;

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName
        {
            get
            {
                return methodName;
            }

            set
            {
                methodName = value;
                SetBaseTypeProperties("methodName", value);
            }
        }

        private string title;

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

        private LogLevel level;

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

                LogEventInfo.Level = nlogLevel;

                SetBaseTypeProperties("logLevel", nlogLevel);
            }
        }

        private string message;

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
                LogEventInfo.Message = Title + ":" + value;

                SetBaseTypeProperties("logMessage", value);
            }
        }

        private string logStackTrace;

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
                LogEventInfo.Message = value;

                SetBaseTypeProperties("logStackTrace", value);
            }
        }

        private DateTime logTime;

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
                LogEventInfo.TimeStamp = value;

                SetBaseTypeProperties("logTime", value);
            }
        }

        private string userId;

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

        #endregion

        /// <summary>
        /// 当设置LogInfo的属性时，会调用该方法，同时设置LogEventInfo对象的Properties字典的值
        /// 对应日志配置文件的event-context参数
        /// </summary>
        /// <param name="key">跟配置文件的key一样</param>
        /// <param name="value">对应的值</param>
        protected void SetBaseTypeProperties(string key, object value)
        {
            LogEventInfo.Properties[key] = value;
        }
    }
}
