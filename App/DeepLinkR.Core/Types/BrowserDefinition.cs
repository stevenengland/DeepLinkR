using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeepLinkR.Core.Types
{
	[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class BrowserDefinition
	{
		[JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty(Required = Required.Always)]
		public string PathToExecutable { get; set; }

		[JsonProperty(Required = Required.Default)]
		public string CommandlineArguments { get; set; }

		[JsonProperty(Required = Required.Default)]
		public bool IsDefault { get; set; }
	}
}
