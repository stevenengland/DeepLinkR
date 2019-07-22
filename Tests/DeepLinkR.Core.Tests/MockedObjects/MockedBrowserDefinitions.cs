using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Types;

namespace DeepLinkR.Core.Tests.MockedObjects
{
	public static class MockedBrowserDefinitions
	{
		public static List<BrowserDefinition> BrowserDefinitions { get; set; } = new List<BrowserDefinition>()
		{
			new BrowserDefinition
			{
				Name = "test",
				IsDefault = true,
			},
		};

		public static List<BrowserDefinition> BrowserDefinitionsWithoutDefaultBrowser { get; set; } = new List<BrowserDefinition>()
		{
			new BrowserDefinition
			{
				Name = "test",
			},
		};
	}
}
