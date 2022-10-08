using System;

namespace Framework
{
    /// <summary>
    /// 调试器
    /// </summary>
    public class Debugger : UnityEngine.ILogHandler
    {
        public enum LogLevel : int
        {
            Log = 0,
            Warning = 1,
            Assert = 2,
            Exception = 3,
            Error = 4,
            None = 5,
        }

        #region Variable
        /// <summary>
        /// 实例
        /// </summary>
        static Debugger m_instance = new Debugger();

        /// <summary>
        /// 日志的等级
        /// </summary>
        static LogLevel m_logLevel = LogLevel.Log;
        #endregion

        #region Property
        /// <summary>
        /// 是否需要日志
        /// </summary>
        public static bool logEnabled
        {
            get
            {
                return UnityEngine.Debug.unityLogger.logEnabled;
            }
            set
            {
                UnityEngine.Debug.unityLogger.logEnabled = value;
            }
        }

        /// <summary>
        /// 日志的等级
        /// </summary>
        public static LogLevel logLevel
        {
            get { return m_logLevel; }
            set { m_logLevel = value; }
        }

        /// <summary>
        /// 原始LogHandler
        /// </summary>
        public static UnityEngine.ILogHandler logHandler
        {
            get; set;
        }
        #endregion

        #region Function
        /// <summary>
        /// 私有构造
        /// </summary>
        private Debugger() {}

        /// <summary>
        /// 日志开启
        /// </summary>
        public static void Start(LogLevel logLevel)
        {
            m_logLevel = logLevel;
            if (null == logHandler)
            {
#if !UNITY_EDITOR
                //logHandler = UnityEngine.Debug.unityLogger.logHandler;
                //UnityEngine.Debug.unityLogger.logHandler = m_instance;
#endif
            }
            logEnabled = LogLevel.None != m_logLevel;
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            if (m_logLevel <= LogLevel.Exception)
            {
#if UNITY_EDITOR
                logHandler?.LogException(exception, context);
#else

#endif
            }
        }

        public void LogFormat(UnityEngine.LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (m_logLevel <= GetLogLevel(logType))
            {
#if UNITY_EDITOR
                logHandler?.LogFormat(logType, context, format, args);
#else

#endif
            }
        }

        private LogLevel GetLogLevel(UnityEngine.LogType logType)
        {
            switch (logType)
            {
                case UnityEngine.LogType.Error: return LogLevel.Error;
                case UnityEngine.LogType.Assert: return LogLevel.Assert;
                case UnityEngine.LogType.Warning: return LogLevel.Warning;
                case UnityEngine.LogType.Log: return LogLevel.Log;
                case UnityEngine.LogType.Exception: return LogLevel.Exception;
                default: return LogLevel.None;
            }
        }
        #endregion
    }
}