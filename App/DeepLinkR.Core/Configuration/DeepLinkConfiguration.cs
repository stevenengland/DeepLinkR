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
	public class DeepLinkConfiguration : IDeepLinkConfiguration
	{
		[JsonConstructor]
		public DeepLinkConfiguration()
		{
		}

		[JsonProperty(Required = Required.Always)]
		public List<DeepLinkCategory> DeepLinkCategories { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public List<DeepLinkKey> DeepLinkKeys { get; private set; }
	}
}
