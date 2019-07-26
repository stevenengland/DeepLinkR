using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeepLinkR.Core.Configuration
{
	[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class AppConfiguration : IAppConfiguration
	{
		[JsonConstructor]
		public AppConfiguration(
			BrowserConfiguration browserConfiguration,
			ClipboardConfiguration clipboardConfiguration,
			LoggingConfiguration loggingConfiguration)
		{
			this.BrowserConfiguration = browserConfiguration;
			this.ClipboardConfiguration = clipboardConfiguration;
			this.LoggingConfiguration = loggingConfiguration;
		}

		[JsonProperty(Required = Required.Always)]
		public IBrowserConfiguration BrowserConfiguration { get; set; }

		[JsonProperty(Required = Required.Always)]
		public IClipboardConfiguration ClipboardConfiguration { get; }

		[JsonProperty(Required = Required.Always)]
		public ILoggingConfiguration LoggingConfiguration { get; }
	}
}
