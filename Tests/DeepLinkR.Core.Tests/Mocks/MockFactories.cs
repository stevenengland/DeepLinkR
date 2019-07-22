using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepLinkR.Core.Configuration;
using DeepLinkR.Core.Helper.LibraryMapper.SharpClipboardMapper;
using DeepLinkR.Core.Helper.LibraryMapper.TextCopyMapper;
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

		public static ClipboardManager ClipboardManagerFactory(Dictionary<string, object> mockObjects)
		{
			return new ClipboardManager(
				(IClipboardConfiguration)mockObjects[nameof(IClipboardConfiguration)],
				(ISharpClipboardMapper)mockObjects[nameof(ISharpClipboardMapper)],
				(ITextCopyMapper)mockObjects[nameof(ITextCopyMapper)]);
		}

		public static Dictionary<string, object> GetMockObjects()
		{
			return new Dictionary<string, object>()
			{
				{ nameof(IBrowserConfiguration), MockFactories.GetBrowserConfiguration() },
				{ nameof(IProcessProxy), MockFactories.GetProcessProxy() },
				{ nameof(IBrowserManager), MockFactories.GetBrowserManager() },
				{ nameof(IClipboardConfiguration), MockFactories.GetClipboardConfiguration() },
				{ nameof(ISharpClipboardMapper), MockFactories.GetSharpClipboardMapper() },
				{ nameof(ITextCopyMapper), MockFactories.GetTextCopyMapper() },
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

		public static IClipboardConfiguration GetClipboardConfiguration()
		{
			return new Mock<IClipboardConfiguration>().Object;
		}

		public static ISharpClipboardMapper GetSharpClipboardMapper()
		{
			return new Mock<ISharpClipboardMapper>().Object;
		}

		public static ITextCopyMapper GetTextCopyMapper()
		{
			return new Mock<ITextCopyMapper>().Object;
		}
	}
}
