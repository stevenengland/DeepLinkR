using System;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Types.Enums;
using NLog;
using NLog.Config;
using LogLevel = NLog.LogLevel;

namespace DeepLinkR.Core.Services.LoggerManager
{
	public class LoggerManager : Logger, ILoggerManager
	{
		private const string _loggerName = "NLogLogger";

		public static ILoggerManager GetLoggingService(ILoggingConfiguration loggingConfiguration)
		{
			var config = new NLog.Config.LoggingConfiguration();

			LogLevel minLogLevel = LogLevel.Error;
			switch (loggingConfiguration.LogLevel)
			{
				case Types.Enums.LogLevel.Info:
					minLogLevel = LogLevel.Info;
					break;
				case Types.Enums.LogLevel.Trace:
					minLogLevel = LogLevel.Trace;
					break;
				case Types.Enums.LogLevel.Debug:
					minLogLevel = LogLevel.Debug;
					break;
				case Types.Enums.LogLevel.Warn:
					minLogLevel = LogLevel.Warn;
					break;
				case Types.Enums.LogLevel.Error:
					minLogLevel = LogLevel.Error;
					break;
				case Types.Enums.LogLevel.Fatal:
					minLogLevel = LogLevel.Fatal;
					break;
			}

			var logfile = new NLog.Targets.FileTarget("logfile")
			{
				FileName = "log.txt",
				DeleteOldFileOnStartup = true,
			};

			if (loggingConfiguration.LogVerbosity == LogVerbosity.Quit)
			{
				logfile.Layout =
					"Local-Date=${longdate}|Level=${level}|Log-Message=${message}|Error-Message=${event-context:item=error-message}";
			}
			else if(loggingConfiguration.LogVerbosity == LogVerbosity.Diagnostic)
			{
				logfile.Layout =
					"Local-Date=${longdate}|Level=${level}|Log-Message=${message}|Error-Source=${event-context:item=error-source}|Error-Class=${event-context:item=error-class}|Error-Method=${event-context:item=error-method}|Error-Message=${event-context:item=error-message}|Inner-Error-Message=${event-context:item=inner-error-message}";
			}
			else
			{
				logfile.Layout =
					"Local-Date=${longdate}|Level=${level}|Log-Message=${message}|Error-Source=${event-context:item=error-source}|Error-Class=${event-context:item=error-class}|Error-Method=${event-context:item=error-method}|Error-Message=${event-context:item=error-message}|Inner-Error-Message=${event-context:item=inner-error-message}|Stack-Trace=${event-context:item=stack-trace}";
			}

			config.AddRule(minLogLevel, LogLevel.Fatal, logfile);

			// Apply config
			NLog.LogManager.Configuration = config;

			ConfigurationItemFactory.Default.LayoutRenderers
				.RegisterDefinition("utc_date", typeof(UtcDateRenderer));

			if (!loggingConfiguration.IsLoggingEnabled)
			{
				LogManager.GlobalThreshold = LogLevel.Off;
			}

			ILoggerManager logger = (ILoggerManager)NLog.LogManager.GetLogger("NLogLogger", typeof(LoggerManager));

			return logger;
		}

		public void Debug(Exception exception, string format, params object[] args)
		{
			if (!this.IsDebugEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Debug, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Error(Exception exception, string format, params object[] args)
		{
			if (!base.IsErrorEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Error, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Fatal(Exception exception, string format, params object[] args)
		{
			if (!base.IsFatalEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Fatal, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Info(Exception exception, string format, params object[] args)
		{
			if (!base.IsInfoEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Info, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Trace(Exception exception, string format, params object[] args)
		{
			if (!base.IsTraceEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Trace, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Warn(Exception exception, string format, params object[] args)
		{
			if (!base.IsWarnEnabled) return;
			var logEvent = this.GetLogEvent(_loggerName, LogLevel.Warn, exception, format, args);
			base.Log(typeof(ILoggerManager), logEvent);
		}

		public void Debug(Exception exception)
		{
			this.Debug(exception, string.Empty);
		}

		public void Error(Exception exception)
		{
			this.Error(exception, string.Empty);
		}

		public void Fatal(Exception exception)
		{
			this.Fatal(exception, string.Empty);
		}

		public void Info(Exception exception)
		{
			this.Info(exception, string.Empty);
		}

		public void Trace(Exception exception)
		{
			this.Trace(exception, string.Empty);
		}

		public void Warn(Exception exception)
		{
			this.Warn(exception, string.Empty);
		}

		private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
		{
			string assemblyProp = string.Empty;
			string classProp = string.Empty;
			string methodProp = string.Empty;
			string messageProp = string.Empty;
			string innerMessageProp = string.Empty;
			string stackTraceProp = string.Empty;

			var logEvent = new LogEventInfo
				(level, loggerName, string.Format(format, args));

			if (exception != null)
			{
				assemblyProp = exception.Source;
				classProp = exception.TargetSite.DeclaringType.FullName;
				methodProp = exception.TargetSite.Name;
				messageProp = exception.Message;
				stackTraceProp = exception.StackTrace;

				if (exception.InnerException != null)
				{
					innerMessageProp = exception.InnerException.Message;
				}
			}

			logEvent.Properties["error-source"] = assemblyProp;
			logEvent.Properties["error-class"] = classProp;
			logEvent.Properties["error-method"] = methodProp;
			logEvent.Properties["error-message"] = messageProp;
			logEvent.Properties["inner-error-message"] = innerMessageProp;
			logEvent.Properties["stack-trace"] = stackTraceProp;

			return logEvent;
		}
	}
}
