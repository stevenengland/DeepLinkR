using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DeepLinkR.Core.Configuration
{
	[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class LoggingConfiguration : ILoggingConfiguration
	{
		[JsonProperty(Required = Required.Default)]
		[DefaultValue(false)]
		public bool IsLoggingEnabled { get; private set; }

		[JsonProperty(Required = Required.Default)]
		[DefaultValue(Types.Enums.LogLevel.Error)]
		[JsonConverter(typeof(StringEnumConverter))]
		public LogLevel LogLevel { get; private set; }

		[JsonProperty(Required = Required.Default)]
		[DefaultValue(Types.Enums.LogVerbosity.Quit)]
		[JsonConverter(typeof(StringEnumConverter))]
		public LogVerbosity LogVerbosity { get; private set; }
	}
}
