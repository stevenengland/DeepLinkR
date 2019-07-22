using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Services.BrowserManager;
using DeepLinkR.Core.Services.ClipboardManager;
using DeepLinkR.Core.Services.ProcessProxy;
using Moq;

namespace DeepLinkR.Core.Tests.Mocks
{
	public static class MockFactories
	{
		public static BrowserManager BrowserManagerFactory(Dictionary<string, object> mockObjects)
		{
			return new BrowserManager(
				(IBrowserConfiguration)mockObjects[nameof(IBrowserConfiguration)],
				(IProcessProxy)mockObjects[nameof(IProcessProxy)]);
		}

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IBrowserConfiguration), MockFactories.GetBrowserConfiguration() },
				{ nameof(IProcessProxy), MockFactories.GetProcessProxy() },
			};
		}

		private static IBrowserConfiguration GetBrowserConfiguration()
		{
			return new Mock<IBrowserConfiguration>().Object;
		}

		public static IBrowserManager GetBrowserManager()
		{
			return new Mock<IBrowserManager>().Object;
		}

		public static IProcessProxy GetProcessProxy()
		{
			return new Mock<IProcessProxy>().Object;
		}
	}
}
