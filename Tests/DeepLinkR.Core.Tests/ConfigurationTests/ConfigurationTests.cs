using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace DeepLinkR.Core.Tests.ConfigurationTests
{
	public class ConfigurationTests
	{
		private readonly Configuration.ConfigurationCollection config;

		public ConfigurationTests()
		{
			using (var streamReader = new StreamReader("config.json"))
			{
				var jsonString = streamReader.ReadToEnd();
				this.config = JsonConvert.DeserializeObject<ConfigurationCollection>(jsonString);
			}
		}

		[Fact]
		public void LoadingConfigurationSucceeds()
		{
			// Simple check for a loaded config.
			Assert.NotNull(this.config);

			// Because Config consists of interfaces and needs constructor injection, check if the interfaces are filled.
			Assert.NotNull(this.config.DeepLinkConfiguration);
			Assert.NotNull(this.config.AppConfiguration);
			Assert.NotNull(this.config.AppConfiguration.BrowserConfiguration);
			Assert.NotNull(this.config.AppConfiguration.ClipboardConfiguration);
			Assert.NotNull(this.config.AppConfiguration.LoggingConfiguration);

			// The rest is controlled via Json Required attribute->Missing or false config leads to an exception.
		}
	}
}
