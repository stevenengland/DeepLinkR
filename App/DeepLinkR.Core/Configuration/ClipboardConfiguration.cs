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
	public class ClipboardConfiguration : IClipboardConfiguration
	{
		[JsonProperty(Required = Required.Default)]
		public bool AutomaticTrim { get; private set; } = false;

		[JsonProperty(Required = Required.Default)]
		public bool ProcessMultipleRows { get; private set; } = false;
	}
}
