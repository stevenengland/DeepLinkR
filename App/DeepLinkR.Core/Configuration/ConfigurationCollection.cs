﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeepLinkR.Core.Configuration
{
	public class ConfigurationCollection : IConfigurationCollection
	{
		[JsonConstructor]
		public ConfigurationCollection(
			DeepLinkConfiguration deepLinkConfiguration,
			AppConfiguration appConfiguration)
		{
			this.DeepLinkConfiguration = deepLinkConfiguration;
			this.AppConfiguration = appConfiguration;
		}

		[JsonProperty(Required = Required.Always)]
		public IAppConfiguration AppConfiguration { get; set; }

		[JsonProperty(Required = Required.Always)]
		public IDeepLinkConfiguration DeepLinkConfiguration { get; set; }
	}
}
