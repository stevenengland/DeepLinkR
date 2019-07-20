using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeepLinkR.Core.Configuration
{
	[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class BrowserConfiguration : IBrowserConfiguration
	{
		[JsonConstructor]
		public BrowserConfiguration()
		{
		}

		[JsonProperty(Required = Required.Always)]
		public List<BrowserDefinition> BrowserDefinitions { get; }
	}
}
